using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryRushExam.Data;
using UnityEngine;

#if DELIVERY_RUSH_UGS
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
#endif

namespace DeliveryRushExam.Save
{
    public class UgsCloudSaveService : ISaveService
    {
        private const string ProgressKey = "delivery_rush_progress";

        public async Task<PlayerProgressData> LoadAsync()
        {
#if DELIVERY_RUSH_UGS
            try
            {
                Dictionary<string, Item> results = await CloudSaveService.Instance.Data.Player.LoadAsync(
                    new HashSet<string> { ProgressKey }
                );

                if (results.TryGetValue(ProgressKey, out Item item))
                {
                    PlayerProgressData data = item.Value.GetAs<PlayerProgressData>();
                    return data ?? new PlayerProgressData();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Cloud Load failed, returning default. " + e.Message);
            }
            return new PlayerProgressData();
#else
            Debug.LogWarning("UGS Cloud Save is not enabled. Add UGS packages and define DELIVERY_RUSH_UGS.");
            await Task.Yield();
            return new PlayerProgressData();
#endif
        }

        public async Task SaveAsync(PlayerProgressData progressData)
        {
#if DELIVERY_RUSH_UGS
            try
            {
                progressData.TouchSaveDate();
                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { ProgressKey, progressData }
                };
                await CloudSaveService.Instance.Data.Player.SaveAsync(data);
                Debug.Log("Cloud Save: progress saved. Date: " + progressData.lastSaveDate);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Cloud Save failed. " + e.Message);
            }
#else
            Debug.LogWarning("UGS Cloud Save is not enabled. Add UGS packages and define DELIVERY_RUSH_UGS.");
            await Task.Yield();
#endif
        }
    }
}
