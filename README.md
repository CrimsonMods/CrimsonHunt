# CrimsonHunt
`Server side only` mod to create battle-pass like rewards for hunting V Bloods.

Give players rewards for hunting down V Bloods! It can be items or even buffs! 

There is even leaderboard functionality for rankings. As well you can create Hunt Score XP Bonus Events during which players receive extra points.

## Installation
* Install [BepInEx](https://v-rising.thunderstore.io/package/BepInEx/BepInExPack_V_Rising/)
* Install [Bloodstone](https://github.com/decaprime/Bloodstone/releases/tag/v0.2.1)
* Install [Bloody.Core](https://thunderstore.io/c/v-rising/p/Trodi/BloodyCore/)
* Extract _CrimsonHunt_ into _(VRising server folder)/BepInEx/plugins_

## Configurable Values
```ini
[Config]

## Enable or disable the mod
# Setting type: Boolean
# Default value: true
EnableMod = true

## Enable or disable v blood hunting
# Setting type: Boolean
# Default value: true
EnableHunt = true
```

Here is an example of the hunt_levels.json file that is generated, showing both rewarding Buffs and giving Item rewards.
```json
[
  {
    "Level": 1,
    "ExpNeeded": 50,
    "HuntReward": {
      "EffectHash": 0,
      "ItemHash": -152327780,
      "ItemQuantity": 1
    },
    "Message": "{player} has reached {level}!"
  },
  {
    "Level": 2,
    "ExpNeeded": 150,
    "HuntReward": {
      "EffectHash": -1703886455,
      "ItemHash": 0,
      "ItemQuantity": 0
    },
    "Message": "{player} has reached {level}!"
  },
  {
    "Level": 3,
    "ExpNeeded": 350,
    "HuntReward": {
      "EffectHash": 0,
      "ItemHash": 576389135,
      "ItemQuantity": 400
    },
    "Message": "{player} has reached {level}!"
  },
  {
    "Level": 4,
    "ExpNeeded": 50,
    "HuntReward": {
      "EffectHash": -238197495,
      "ItemHash": 0,
      "ItemQuantity": 0
    },
    "Message": "{player} has reached {level}!"
  }
]
```
## Support

Want to support my V Rising Mod development? 

Donations Accepted with [Ko-Fi](https://ko-fi.com/skytech6)

Or buy/play my games! 

[Train Your Minibot](https://store.steampowered.com/app/713740/Train_Your_Minibot/) 

[Boring Movies](https://store.steampowered.com/app/1792500/Boring_Movies/)
