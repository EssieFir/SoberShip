## v1.3.0
* Added a feature to bring back the Vain Shrouds in v60 (Requested by [MiraakThuri](https://github.com/EssieFir/SoberShip/issues/2))
* Added a feature to prevent the Vain Shrouds being reset when they fail to spawn.
* Added a feature to cap Vain Shroud iterations if it's too high.
* Updated config descriptions to describe what they do in v60.

## v1.2.1
* Fixed an issue where the level would never start if a non-host had the mod installed and vain shrouds tried to spawn.
* Removed the setting "RemoveRemaining" as the setting it depended on does the same thing.

## v1.2.0
* SoberShip now relocates the starting position of Vain Shrouds
* Added more config options
  * Option to Disable Vain Shrouds Completely (defaults to false)
    * If the above is enabled, option to remove remaining Vain Shrouds (defaults to true)
  * Options for existing/spreading Vain Shroud Removal
    * Whether the module is enabled (defaults to true)
    * The minimum distance Vain Shrouds are allowed to be (defaults to 35)
    * The method of removal (defaults to DELETION)
  * Options for relocating the start position of Vain Shrouds
    * Whether the module is enabled (defaults to true)
    * The minimum distance the Vain Shroud starting position is allowed to be (defaults to 35)

## v1.1.0
* Now removes newly generated Vain Shrouds as well.

## v1.0.0
* Created the mod