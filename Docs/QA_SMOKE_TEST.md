# QA Smoke Test — Idle/Tycoon MVP

Follow this script to validate the MVP acceptance criteria in ~10 minutes.

## Prep
- Open `Scenes/Bootstrap.unity`
- Ensure platform = Android (not required for Play Mode)
- Make sure `Resources/Config/EconomyConfig.asset` exists with defaults

## 1) Core Loop
- Press **Play**
- Observe Coins increasing (base income/sec)
- Click/Tap the main tap area: Coins increase by tap value
- Buy **Income** upgrade 3–5 times → income/sec rises visibly
- Buy **Tap** upgrade 3–5 times → tap value rises

**Pass:** Currency UI updates live; Number formatting shows K/M once large enough.

## 2) Milestones
- Level an upgrade past a milestone threshold (default every 25 levels; you can temporarily lower via Remote Config default)
- Verify a **global income boost** applies

**Pass:** Income/sec jumps when crossing a milestone.

## 3) Offline Earnings
- Click **Debug Panel** (DEV define) or use the fast‑forward control to simulate 1 hour (or stop Play and set `lastQuit` in Save for a later run)
- Re‑enter game → Offline popup appears with capped earnings

**Pass:** Rewards respect cap & factor; analytics logs event (in Console).

## 4) Prestige
- Accumulate some `totalLifetimeCoins` (you can grant via Debug Panel)
- Open Prestige Panel → shows **shards to gain**
- Confirm Prestige → upgrades reset; shards increase; global multiplier updated

**Pass:** Post‑prestige income is higher due to multiplier.

## 5) Rewarded Ads (Test Mode)
- Ensure AdMob test unit ID is set in `AdConfig.asset`
- Press **2× Income (4:00)** → watch ad → buff timer appears and counts down
- Press **+1 Shard** → up to daily cap; cooldown respected

**Pass:** Rewards granted, cooldowns & caps enforced.

## 6) IAP (Sandbox / Stub)
- Open IAP UI → trigger mock purchase in Editor (or test on device via Play Console Internal Track)
- Verify entitlements:
  - **Remove Ads** flag stored & reflected
  - **Starter Pack** grants Gems, removes ads, applies the boost

**Pass:** Entitlements persist via SaveSystem; UI state updated.

## 7) Remote Config
- Trigger **RC Fetch** in Debug Panel (or rely on startup)
- Change a knob (e.g., growth_income) → refetch → check that economy reflects new value

**Pass:** Changes take effect without restart; defaults used if fetch fails.

## 8) Tests
- Open **Test Runner** → run **EditMode** then **PlayMode** tests

**Pass:** All tests green.

---

If any step fails: capture the Console log, note repro steps, and file an issue with expected vs. actual behavior.
