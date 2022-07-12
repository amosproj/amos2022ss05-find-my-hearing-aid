# Explorative Tests
Testing on main branch from 11/07/22

## Navigation

> Note: these features are also covered by the UI-Tests. They are not working on iOS though, so it makes sense to cover them here again.

1. Start from About/Home page

| Use-Case                                                                 | Works on iOS | Works on Android | Improvement proposal |
|--------------------------------------------------------------------------|--------------|------------------|----------------------|
| You can navigate to Signal Strength Search via Button and back           |              | X                | -                    |
| You can navigate to Geolocation Search via Button and back: works fine   |              | X                | -                    |
| You can navigate to each other page via burger menu and back: works fine |              | X                | -                    |

2. Start from other page

| Use-Case                                                                 | Works on iOS | Works on Android | Improvement proposal |
|--------------------------------------------------------------------------|--------------|------------------|----------------------|
| You can navigate to each other page via burger menu and back: works fine |              | X                | -                    |

## General

| Use-Case                                                                            | Works on iOS | Works on Android | Improvement proposal |
|-------------------------------------------------------------------------------------|--------------|------------------|----------------------|
| If I have Bluetooth or GPS deactivated on my phone, I want to get notified about it |              | X                | -                    |


## Scan for available Devices

| Use-Case                                                                                                               | Works on iOS | Works on Android | Improvement proposal                                                                                                                                                                           |
|------------------------------------------------------------------------------------------------------------------------|--------------|------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| If there's no BLE device in range the user should be notified                                                          |              |                  | Currently it doesn't show anything => Show loading symbol after button is pressed and notify user when nothing is found                                                                        |
| The user shouldn't add multiple devices with the same label                                                            |              |                  | Currently it's possible to save two devices with the same label => Prevent that with an error message in adding and renaming device                                                            |
| The number of letters for the Device-label is limited                                                                  |              |                  | Currently it's possible to fill the label with unlimited (I think) letters, resulting in a bad looking UI => Limit the number of letters in add and rename label to 15 letters                 |
| After adding a device the other possible (not yet added) devices will still be displayed                               |              |                  | Currently when adding a new device all other devices that could possibly be added are gone and you have to press the button again => Keep the other possible devices                           |
| After canceling a device, you wanted to add before, the other possible (not yet added) devices will still be displayed |              |                  | Currently when canceling a device, you wanted to add before, all other devices that could possibly be added are gone and you have to press the button again => Keep the other possible devices |

## Saved devices

| Use-Case                                                                                                          | Works on iOS | Works on Android | Improvement proposal                                                                                                       |
|-------------------------------------------------------------------------------------------------------------------|--------------|------------------|----------------------------------------------------------------------------------------------------------------------------|
| I want to immediately see, which of my devices my phone is "seeing"                                               |              | X                | -                                                                                                                          |
| I want to be able to go into the Strength Search directly from within the devices-page with a device pre-selected |              | X                | -                                                                                                                          |
| I want to be able to go into the MapSearch directly from within the devices-page with a device pre-selected       |              | X                |                                                                                                                            |
| I want to have a sensible view of my saved devices                                                                |              | X                | The scrollview can be scrolled down quite a bit, even if only one device is listed in it. Deactivate scroll if not needed? |