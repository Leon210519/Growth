# Growth Idle Tycoon

Unity 2022.3 URP 2D project targeting Android portrait. Core loop: tap + idle income, upgrades, prestige, rewarded ads, and IAP.

## Getting Started
1. Open with Unity 2022.3 LTS or newer.
2. Scenes: load `Scenes/Bootstrap.unity` for startup.
3. Player Settings: Android, IL2CPP, ARM64, min SDK 24, portrait only, VSync disabled.

## Services Configuration
- **AdMob**: Replace IDs in `Resources/Config/AdConfig.asset`.
- **Unity IAP**: Configure product metadata in `Resources/Config/IAPConfig.asset` and set up in Unity Dashboard.
- **Firebase**: Hook `RemoteConfigService` and `AnalyticsService` into Firebase SDK.

## Testing
Run EditMode and PlayMode tests via Unity Test Runner.

## Debugging
Define scripting symbol `DEV` to enable in-game debug console and panel.

## Privacy
See `PRIVACY_POLICY_TEMPLATE.md` for store submission.
