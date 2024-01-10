using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialSys.UnitySDK
{
    public static class SpatialBridge
    {
        public static IActorService actorService;
        public static ICameraService cameraService;
        public static ICoreGUIService coreGUIService;
        public static IMarketplaceService marketplaceService;
        public static INetworkingService networkingService;
        public static IUserWorldDataStoreService userWorldDataStoreService;

        #region ISpaceService

        public delegate int GetSpacePackageVersionDelegate();
        public static GetSpacePackageVersionDelegate GetSpacePackageVersion;

        public delegate bool IsInSandboxDelegate();
        public static IsInSandboxDelegate IsInSandbox;

        public delegate bool HasLocalLovedSpaceDelegate();
        public static HasLocalLovedSpaceDelegate HasLocalLovedSpace;

        public delegate void OpenURLDelegate(string url);
        public static OpenURLDelegate OpenURL;

        public delegate void EnableAvatarToAvatarCollisionsDelegate(bool enabled);
        public static EnableAvatarToAvatarCollisionsDelegate EnableAvatarToAvatarCollisions;

        #endregion

        #region ILoggingService

        public delegate void LogErrorDelegate(string message, Exception ex = null, Dictionary<string, object> data = null);
        public static LogErrorDelegate LogError;

        #endregion

        #region IQuestSystemService
        public delegate void QuestDelegate(SpatialQuest quest);
        public static QuestDelegate StartQuest;
        public static QuestDelegate CompleteQuest;
        public static QuestDelegate ResetQuest;

        public delegate void QuestTaskDelegate(SpatialQuest quest, uint taskID);
        public static QuestTaskDelegate StartQuestTask;
        public static QuestTaskDelegate CompleteQuestTask;
        public delegate void QuestTaskProgressDelegate(SpatialQuest quest, uint taskID, int progress);
        public static QuestTaskProgressDelegate AddQuestTaskProgress;
        public static QuestTaskProgressDelegate SetQuestTaskProgress;

        public delegate int GetQuestTaskProgressDelegate(SpatialQuest quest, uint taskID);
        public static GetQuestTaskProgressDelegate GetQuestTaskProgress;

        public delegate int GetQuestStatusDelegate(SpatialQuest quest);
        public static GetQuestStatusDelegate GetQuestStatus;

        public delegate int GetQuestTaskStatusDelegate(SpatialQuest quest, uint taskID);
        public static GetQuestTaskStatusDelegate GetQuestTaskStatus;
        #endregion

        #region IBadgeService
        public delegate void RewardBadgeDelegate(string badgeID);
        public static RewardBadgeDelegate RewardBadge;
        #endregion

        #region IInventoryService
        // Backpack
        public delegate void AddBackpackItemDelegate(string itemID, ulong quantity, bool showToast, Action<bool> callback);
        public static AddBackpackItemDelegate AddBackpackItem;

        public delegate void DeleteBackpackItemDelegate(string itemID, Action<bool> callback);
        public static DeleteBackpackItemDelegate DeleteBackpackItem;

        public struct GetBackpackItemResponse
        {
            public bool userOwnsItem;
            public ulong amount;
        }
        public delegate void GetBackpackItemDelegate(string itemID, Action<SpatialBridge.GetBackpackItemResponse> callback);
        public static GetBackpackItemDelegate GetBackpackItem;

        public delegate void UseBackpackItemDelegate(string itemID, Action<bool> callback);
        public static UseBackpackItemDelegate UseBackpackItem;

        public delegate void SetBackpackItemEnabledDelegate(string itemID, bool enabled, string disabledMessage);
        public static SetBackpackItemEnabledDelegate SetBackpackItemEnabled;

        public delegate void SetBackpackItemTypeEnabledDelegate(ItemType itemType, bool enabled, string disabledMessage);
        public static SetBackpackItemTypeEnabledDelegate SetBackpackItemTypeEnabled;

        // Consumables
        public struct GetConsumableItemStateResponse
        {
            public bool isActive;
            public float durationRemaining;
            public bool onCooldown;
            public float cooldownRemaining;
        }
        public delegate void GetConsumableItemStateDelegate(string itemID, Action<GetConsumableItemStateResponse> callback);
        public static GetConsumableItemStateDelegate GetConsumableItemState;

        // World Currency
        public delegate ulong GetWorldCurrencyBalanceDelegate();
        public static GetWorldCurrencyBalanceDelegate GetWorldCurrencyBalance;

        public delegate void AwardWorldCurrencyDelegate(ulong amount, Action<bool> callback);
        public static AwardWorldCurrencyDelegate AwardWorldCurrency;
        #endregion

        #region ISceneService

        // Avatar Attachments
        public delegate int GetActorFromAvatarAttachmentObjectDelegate(SpatialAvatarAttachment attachment);
        public static GetActorFromAvatarAttachmentObjectDelegate GetActorFromAvatarAttachmentObject;

        // Prefab Objects
        public delegate void SpawnPrefabObjectFromPackageDelegate(string sku, Vector3 position, Quaternion rotation);
        public static SpawnPrefabObjectFromPackageDelegate SpawnPrefabObjectFromPackage;

        public delegate void SpawnPrefabObjectFromEmbeddedDelegate(string assetID, Vector3 position, Quaternion rotation);
        public static SpawnPrefabObjectFromEmbeddedDelegate SpawnPrefabObjectFromEmbedded;

        //Synced objects
        public delegate bool TakeoverSyncedObjectOwnerhipDelegate(SpatialSyncedObject syncedObject);
        public static TakeoverSyncedObjectOwnerhipDelegate TakeoverSyncedObjectOwnership;

        public delegate SpatialSyncedObject GetSyncedObjectByIDDelegate(int id);
        public static GetSyncedObjectByIDDelegate GetSyncedObjectByID;

        public delegate bool GetSyncedObjectIsSyncedDelegate(SpatialSyncedObject syncedObject);
        public static GetSyncedObjectIsSyncedDelegate GetSyncedObjectIsSynced;

        public delegate int GetSyncedObjectIDDelegate(SpatialSyncedObject syncedObject);
        public static GetSyncedObjectIDDelegate GetSyncedObjectID;

        public delegate int GetSyncedObjectOwnerDelegate(SpatialSyncedObject syncedObject);
        public static GetSyncedObjectOwnerDelegate GetSyncedObjectOwner;

        public delegate bool GetSyncedObjectHasControlDelegate(SpatialSyncedObject syncedObject);
        public static GetSyncedObjectHasControlDelegate GetSyncedObjectHasControl;

        public delegate bool GetSyncedObjectIsLocallyOwnedDelegate(SpatialSyncedObject syncedObject);
        public static GetSyncedObjectIsLocallyOwnedDelegate GetSyncedObjectIsLocallyOwned;

        public delegate void SetSyncedAnimatorParameterDelegate(SpatialSyncedAnimator syncedAnimator, string parameterName, object value);
        public static SetSyncedAnimatorParameterDelegate SetSyncedAnimatorParameter;

        public delegate void SetSyncedAnimatorTriggerDelegate(SpatialSyncedAnimator syncedAnimator, string triggerName);
        public static SetSyncedAnimatorTriggerDelegate SetSyncedAnimatorTrigger;

        #endregion

        #region IInputService
        public delegate void SetInputOverridesDelegate(bool movementOverride, bool jumpOverride, bool sprintOverride, bool actionButtonOverride, GameObject target);
        public static SetInputOverridesDelegate SetInputOverrides;

        public delegate void OnInputGraphRootObjectDestroyedDelegate(GameObject target);
        public static OnInputGraphRootObjectDestroyedDelegate OnInputGraphRootObjectDestroyed;

        public delegate void StartVehicleInputCaptureDelegate(VehicleInputFlags flags, Sprite primaryButtonSprite, Sprite secondaryButtonSprite, GameObject target);
        public static StartVehicleInputCaptureDelegate StartVehicleInputCapture;

        public delegate void StartCompleteCustomInputCaptureDelegate(GameObject target);
        public static StartCompleteCustomInputCaptureDelegate StartCompleteCustomInputCapture;

        public delegate void ReleaseInputCaptureDelegate(GameObject target);
        public static ReleaseInputCaptureDelegate ReleaseInputCapture;
        #endregion

        #region ISpatialComponentService
        public delegate void InitializeSpatialInteractableDelegate(SpatialInteractable spatialInteractable);
        public static InitializeSpatialInteractableDelegate InitializeSpatialInteractable;

        public delegate void InitializeSpatialPointOfInterestDelegate(SpatialPointOfInterest spatialPointOfInterest);
        public static InitializeSpatialPointOfInterestDelegate InitializeSpatialPointOfInterest;

        public delegate void PointOfInterestEnabledChangedDelegate(SpatialPointOfInterest spatialPointOfInterest, bool enabled);
        public static PointOfInterestEnabledChangedDelegate PointOfInterestEnabledChanged;

        public delegate bool GetIsSceneInitializedDelegate();
        public static GetIsSceneInitializedDelegate GetIsSceneInitialized;

        public delegate void InitializeSpatialSeatHotspotDelegate(SpatialSeatHotspot spatialHotspot);
        public static InitializeSpatialSeatHotspotDelegate InitializeSpatialSeatHotspot;

        public delegate void InitializeSpatialAvatarTeleporterDelegate(SpatialAvatarTeleporter spatialAvatarTeleporter);
        public static InitializeSpatialAvatarTeleporterDelegate InitializeSpatialAvatarTeleporter;

        public delegate void InitializeSpatialTriggerEventDelegate(SpatialTriggerEvent spatialTriggerEvent);
        public static InitializeSpatialTriggerEventDelegate InitializeSpatialTriggerEvent;

        public delegate void TriggerEventEnabledChangedDelegate(SpatialTriggerEvent spatialTriggerEvent, bool enabled);
        public static TriggerEventEnabledChangedDelegate TriggerEventEnabledChanged;

        public delegate void InitializeSpatialClimbableDelegate(SpatialClimbable climbable);
        public static InitializeSpatialClimbableDelegate InitializeSpatialClimbable;

        public delegate void InitializeSpatialCameraPassthroughDelegate(SpatialCameraPassthrough spatialCameraPassthrough);
        public static InitializeSpatialCameraPassthroughDelegate InitializeSpatialCameraPassthrough;

        public delegate Action InitializeVirtualCameraDelegate(SpatialVirtualCamera virtualCamera);
        public static InitializeVirtualCameraDelegate InitializeSpatialVirtualCamera;
        #endregion

        #region IAudioService
        public delegate void PlaySpatialSFXPositionDelegate(SpatialSFX sfx, Vector3 position, float extraVolume, float extraPitch);
        public static PlaySpatialSFXPositionDelegate PlaySpatialSFXPosition;

        public delegate void PlaySpatialSFXSourceDelegate(SpatialSFX sfx, AudioSource source, float extraVolume, float extraPitch);
        public static PlaySpatialSFXSourceDelegate PlaySpatialSFXSource;
        #endregion

        #region IVFXService
        public delegate void CreateFloatingTextDelegate(string text, FloatingTextAnimStyle style, Vector3 position, Vector3 force, Color color, bool gravity, AnimationCurve scaleCurve, AnimationCurve alphaCurve, float lifetime);
        public static CreateFloatingTextDelegate CreateFloatingText;
        #endregion
    }
}