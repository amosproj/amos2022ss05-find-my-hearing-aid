## Testing RSSI values with Arduino devices

These are the test results of: Leo

| Scenario \ Distance | Outdoors | Indoors (without walls in between) | Indoors (with walls in between) | Comments |
|---|---|---|---|---|
| 1 m | [-65, -70] | [-65, -70] | (1 wall) [-75, -80] |  |
| 2 m | [-80, -90] | [-70, -80] | (1 wall) [-75, -80] |  |
| 5 m | [-85, -90] | [-75, -85] | (2 walls) [-85, -90] |  |
| 10 m | [-85, -95] | [-80, -90] | (3 walls) [-90, -100] |  |

### Other comments

RSSI measurements are quite volatile and heavily dependent on environmental factors, which is why the test results consist of number ranges, rather than specific numbers.

All the measurements were taken with 1 additional Arduino (next to the tested device) and 2 to 4 other, unidentified Bluetooth devices in the vicinity.
Tests without other devices interfering could not be conducted as these other devices were unknown and thus could not be turned off.
However, I don't expect this would have made any significant difference in the measurements.

The RSSI at 1 m distance being the same outdoors and indoors (without walls in between) was expected, as that is how the measured power / txPower of the device is defined.
It may vary depending on the device's surroundings, but in an optimal scenario it doesn't.
(See https://stackoverflow.com/questions/36862185/what-exactly-is-txpower-for-bluetooth-le-and-how-is-it-used)

The RSSI at 2 m distance and further being smaller (in terms of absolute value) indoors (without walls in between) than outdoors can be explained by the surrounding walls reflecting parts of the signal that would otherwise be lost back into the room, thereby increasing the signal strength = decreasing the RSSI (in its absolute value).
With walls in between, this effect is still present at 2 m, but later negated by the walls in between reducing the signal strength more than the surrounding walls can improve it again. 

The larger the RSSI values become (in terms of absolute value), the less frequent they are being updated.
Around -90 to -100, updates sometimes take as long as a second or more, even if the RSSI polling is set to a much higher frequency.

The test results above can now be used to make an informed decision on which value to use for the environmental factor in our RSSI-to-Meter formula (by solving the formula for this factor and plugging in the numbers above).
Once a good environmental factor (and a default value for the measured power / txPower) have been chosen, the estimated distance in meters should be fairly accurate.
