### Title: Sprint Review with the industry partner  
### Date: 20/05/2022  
### Time: 11:00PM - 12:00PM  
### Location: Zoom
### Participants: XC, EW, ~~LM~~, NS, DP, ~~JS~~, AW, ~~MA~~, LK, MR(WSA), SM(WSA)

---
### Goals
Communicate our to-date progress to and gather feedback from the industry partner. 

### Agenda
- POs give a quick feedback on the meeting with WSA's PO.
- Show our progress and question time 
- Review and discussion on CI pipeline and GUI design

### Discussion
- Overall suggestions
  - Separate Gui and Logic (Mvvm design pattern). Specify interfaces first, then developers for frontend and backend can work in parallel. [More info on The Model-View-ViewModel](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/mvvm)
  - Make use of branches on Github, don't work locally, push results regularly for others to see.
- What does "Label" mean? 
  - Name, not the categorization.
  - Human readable name/lable/title, not some ids or unmeaningless string. (E.g. "My earpods")
- Number of device stored on the app.
  - Can start with one device and ends with more than more, but signal and geolocation feature needs to start with two (a pair of) devices.
  - More than two is not mandatory or not the need from the industry partner side.
- How two physical BLEs can be grouped and shown as one device on app?
  - It's realized on some apps on iOS.
  - Do some research and WSA can show us how the My Hearing Aid apps works in reality.
- What are the target user of this app?
  - Not only the old people, but everyone.
- CI pipeline.
  - The local version of app works, but the artifact from the pipeline one has an installation error becasue it is not signed.
    - Requirements: The app should at least be able to be installed on the phone, if there is no certificate then there may be the extra step of agreeing to "do you want to really trust this app?" 
  -  Sonarcould needs admin rights of the Github repository which we don't have. 
  -  Some questions on .NET standards. [Link](https://docs.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0)
  -  Develper acoount on ios.
     -  don't need the developer account of ios, would not be published on app store. 
     -  Suggestion: leave the ios pipeline for now. 
     -  IPA file and certificate generation may need apple developer account. 
- GUI drafts.
  - Showed Leo and Marib's two different designs of the GUI.
  - The decision on which GUI to use depends on the team goals: does it emphasis multiple-devices management (then let the starting page focus on device-list like in Marib's design) or the searching-functionality (then let the starting page focus on search functionalities like in Leo's design)? 
    - It might be a bit difficlut to implement the device-list overlay in Leo's version (making the list hover in from the bottom like that) in the xamarin framework.
    - No need for fancy design at the moment, functionality comes first.
  - Think about hallway testing (optional, if we want to do it):
    - walk around on the university campus and ask random people to try out the app 
    - let actual users test it and get feedback -> Is the app intuitive or confusing? 

### Action items
- [ ] @TEAM: Another meeting with WSA(30min) to show how to seperate business logic from GUI.
- [ ] @TEAM: Another meeting with WSA to discover how their Find My hearing aid app functions.