## Our approach: 
We decided to categorize the settings page to make it more convenient for the user. We priorised it regarding the potential need for the user. 
The Topic should be initially displayed when clickin on the settings icon and Name/Description open up when the user clikcs on the respective topic

<table>
    <thead>
        <tr>
            <th> Topic </th>
            <th> Name </th>
            <th> Description </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td rowspan=6>Customize</td>
            <td>List devices without an advertised name in the devices scan</td>
            <td>Toggle</td>
        </tr>
        <tr>
            <td>List devices with a very weak connection in the devices scan</td>
            <td>Toggle</td>
        </tr>
        <tr>
            <td> The time between RSSI/signal strength updates in milliseconds</td>
            <td>Toggle</td>
        </tr>
        <tr>
            <td>The time between background GPS updates in seconds</td>
            <td>Toggle</td>
        </tr>
        <tr>
            <td>Reset to default values</td>
            <td>Button</td>
        </tr>
        <tr>
            <td>Clear all devices</td>
            <td>Button</td>
        </tr>
        <tr>
            <td rowspan=2>Security</td>
            <td>Current Permissions</td>
            <td>
                Permission | Status <br>
                Read/Write on Filesystem | on/off <br>
                Bluetooth | on/off <br>
                GPS | on/off <br>
                Instead of on/off use deactivated switchs/checkboxes.  <br>
                Implement Hint like: "You're not able to change the permissions within this app, please go to your application settings."
            </td>
        </tr>
        <tr>
            <td>Data policy</td>
            <td>
                - no data sent from mobile device to external source. Everything is locally. <br>
                - no personal data are collected <br>
                - Database: write and read on filesystem if "Read/Write on Filesystem" permission is granted<br>
                - Saving the last location and a timestamp of BLE-devices in database if "GPS" and "Bluetooth" Permission is granted <br>
            </td>
        </tr>
        <tr>
            <td>Help</td>
            <td>Forwarding/Hinting to Guidance Page</td>
            <td>
                Please find any explanation of the functionalties of the software within our specific guidance part which you can find by clicking on the
                white '?' located on the top right corner
            </td>
        </tr>
        <tr>
            <td rowspan=6>About</td>
            <td>Licence</td>
            <td>MIT</td>
        </tr>
        <tr>
            <td>Repository Link</td>
            <td>https://github.com/amosproj/amos2022ss05-find-my-hearing-aid</td>
        </tr>
        <tr>
            <td>Version</td>
            <td>1.0.0</td>
        </tr>
    </tbody>
</table>


DONTS:
- FAQ => Guidance
- Feedback => Implementation
- Infos about AMOS/WSA => too much
