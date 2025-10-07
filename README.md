# Idle/Tycoon Mobile MVP — Developer Setup

This repository contains a Unity 2022.3 LTS project scaffold for an Android idle/tycoon game with rewarded ads, IAP, remote-config driven balancing, unit tests, and documentation. The functional spec lives in **PLAN.txt**. A Codex executor can use **CODEX_TASK.txt** to generate/extend the project without removing existing files.

> **Do not remove existing features. Only extend.**

---

## 1) Requirements
- **Unity 2022.3 LTS (or newer LTS)** with **Android Build Support** (SDK/NDK + OpenJDK).
- (Optional) **Firebase SDK** for Analytics/Remote Config (stubs allow running without).
- (Optional) **Google Mobile Ads SDK** (AdMob). You can start with test IDs.
- (Optional) Git LFS if you plan to store large binaries.

---

## 2) Open the project
1. Clone the repository.
2. Open **Unity Hub → Add project from disk** → select the repo root (Unity will import).
3. Load scene: `Scenes/Bootstrap.unity`. Press **Play**.
4. First run should show the main screen with currencies, three upgrade cards, prestige panel, and two rewarded buttons (with stub behavior if Ads not configured).

---

## 3) Player Settings (Android)
**File → Build Settings → Android → Switch Platform**, then:
- **Scripting Backend**: IL2CPP
- **Target Architectures**: ARM64 (only)
- **Minimum API Level**: Android 7.0 (API 24) or higher
- **Orientation**: Portrait
- **Company Name / Product Name**: set to your values
- **Package Name**: e.g., `com.yourcompany.idle`

---

## 4) Remote Config & Analytics
The game ships with **RemoteConfigService** stubs and **sane defaults** so it runs **without Firebase**.
When you later add Firebase:
- Import Firebase Core + Analytics + Remote Config packages.
- Add `google-services.json` under `Assets/` (as required by your Firebase version).
- Link the project in the Firebase console.
- Remote config keys are listed in `Scripts/Core/Telemetry/RemoteConfigKeys.cs` and also documented in **PLAN.txt**.

> If Firebase is absent, the code uses defaults and logs a warning; gameplay continues.

---

## 5) Ads (Rewarded) — Test Setup
If you don’t add the AdMob SDK yet, the `AdService` returns graceful “no ad available” feedback.  
To quickly test with **AdMob test IDs**:
- Add Google Mobile Ads SDK (UPM or .unitypackage).
- Open `Resources/Config/AdConfig.asset` and set the **Rewarded** unit ID to the Android test ID:  
  `ca-app-pub-3940256099942544/5224354917`
- Play Mode: press a rewarded button → it should show a test ad and grant either **2× income (4m)** or **+1 shard** (daily capped).

> Make sure to use **test IDs** until you ship. Never use real ad units during development.

---

## 6) In‑App Purchases (Unity IAP)
- Install **Unity IAP** via Package Manager.
- Enable IAP (Services/Window) and initialize the catalog with the product IDs from `Scripts/Core/Monetization/IAP/IAPProductIds.cs` (placeholders).
- For Google Play testing, create matching in‑app products in the Play Console, upload an **AAB** to an **Internal** track, and test with a licensed tester account.

> MVP uses **code-driven IAP**, not codeless. Receipt validation is stubbed; add server‑side later if needed.

---

## 7) Tests
Open **Window → General → Test Runner**:
- **EditMode** tests cover balancing helpers and formatting.
- **PlayMode** tests cover economy/earnings logic.
- All tests should pass after Codex generation.

> If you run outside Unity (CI), tests may be skipped; use a Unity Test Runner CI image or run locally.

---

## 8) Build
1. **File → Build Settings → Android → Build** (or Build & Run).
2. Ensure **IL2CPP + ARM64** and **Min API 24**.
3. For Play Console: build **AAB**. Sign with a keystore or use Play App Signing.

---

## 9) Smoke Test (Manual)
See **Docs/QA_SMOKE_TEST.md** for a click‑by‑click script that verifies the MVP Acceptance Criteria.

---

## 10) Troubleshooting
- **Compile errors about Firebase/Ads symbols**: ensure platform guards `#if` blocks are intact; if a package is missing, features fall back to stubs. Re‑import all.
- **Black screen / null references**: open `Scenes/Bootstrap.unity` (not Main directly) so the GameManager bootstraps services.
- **Gradle/Android build failures**: in Package Manager, resolve conflicts; clear `Library/` (reimport) if needed; ensure JDK/SDK are the Unity‑provided ones.
- **Ad not showing**: use AdMob **test** unit IDs; on a real device with internet; check logs for “no fill”.

---

## 11) Branching & Automation
- Work on `codex/idle-tycoon-mvp` branch (or similar).
- **PLAN.txt** is the authoritative spec.
- **CODEX_TASK.txt** can be used to re‑run Codex safely (additive only).

---

## 12) Compliance & Store
Read **Docs/RELEASE_CHECKLIST.md** for:
- Privacy Policy URL
- GDPR/UMP consent flow
- Data Safety form
- Icons/graphics
- Play Integrity / internal testing

---

## Credits
Spec & scaffolding authored collaboratively. Keep code comments in English, gameplay text can be localized later.
