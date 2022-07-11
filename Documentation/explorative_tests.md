# Explorative Tests
Testing on main branch from 11/07/22

## Navigation
1. Start from About/Home page
- You can navigate to Signal Strength Search via Button and back
  - [ ] works on iOS
  - [ ] works on Android
  - [ ] no improvement proposal
- You can navigate to Geolocation Search via Button and back: works fine
  - [ ] works on iOS
  - [ ] works on Android
  - [ ] no improvement proposal
- You can navigate to each other page via burger menu and back: works fine
  - [ ] works on iOS
  - [ ] works on Android
  - [ ] no improvement proposal
2. Start from other page
- You can navigate to each other page via burger menu and back
  - [ ] works on iOS
  - [ ] works on Android
  - [ ] no improvement proposal

## Scan for available Devices
- If there's no BLE device in range the user should be notified
  - [ ] works on iOS
  - [ ] works on Android
  - [ ] improvement proposal: Currently it doesn't show anything => Show loading symbol after button is pressed and notify user when nothing is found
- The user shouldn't add multiple devices with the same label
  - [ ] works on iOS
  - [ ] works on Android
  - [ ] improvement proposal: Currently it's possible to save two devices with the same label => Prevent that with an error message in adding and renaming device
- The number of letters for the Device-label is limited
  - [ ] works on iOS
  - [ ] works on Android
  - [ ] improvement proposal: Currently it's possible to fill the label with unlimited (I think) letters, resulting in a bad looking UI => Limit the number of letters in add and rename label to 15 letters.
- After adding a device the other possible (not yet added) devices will still be displayed
  - [ ] works on iOS
  - [ ] works on Android
  - [ ] improvement proposal: Currently when adding a new device all other devices that could possibly be added are gone and you have to press the button again => Keep the other possible devices
- After canceling a device, you wanted to add before, the other possible (not yet added) devices will still be displayed
  - [ ] works on iOS
  - [ ] works on Android
  - [ ] improvement proposal: Currently when canceling a device, you wanted to add before, all other devices that could possibly be added are gone and you have to press the button again => Keep the other possible devices
