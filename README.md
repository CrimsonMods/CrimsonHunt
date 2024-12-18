# CrimsonHunt
`Server side only` mod to create battle-pass like rewards for hunting V Bloods.

Give players rewards for hunting down V Bloods! It can be items or even buffs! 

There is even leaderboard functionality for rankings. As well you can create Hunt Score XP Bonus Events during which players receive extra points.

### Planned Post-1.1 Update:
- [CrimsonSQL](https://thunderstore.io/c/v-rising/p/skytech6/CrimsonSQL/) Integration (Share leaderboards across your servers)
- [JSONRising](https://thunderstore.io/c/v-rising/p/skytech6/JSONRising/) Support
- Messages.json Feature (some of my other mods have this, lets you build the outputs with optional runtime parameters)

## Installation
* Install [BepInEx](https://v-rising.thunderstore.io/package/BepInEx/BepInExPack_V_Rising/)
* Extract _CrimsonHunt.dll_ into _(VRising server folder)/BepInEx/plugins_

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

## Commands
CrimsonHunt is up for a large update after 1.0. 

For now please use `.help CrimsonHunt` to learn about the available commands.

## Support

Want to support my V Rising Mod development? 

Donations Accepted

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/skytech6)

Or buy/play my games! 

[Train Your Minibot](https://store.steampowered.com/app/713740/Train_Your_Minibot/) 

[Boring Movies](https://store.steampowered.com/app/1792500/Boring_Movies/) **Free to Play!**

**If you are looking to hire someone to make a mod for any Unity game reach out to me on Discord! (skytech6)**
