## Testing RSSI values with Arduino devices

These are the test results of: Leo

| Scenario \ Distance | Outdoors (a few meters away from a building) | Indoors (without walls in between) | Indoors (with walls in between) | Comments |
|---|---|---|---|---|
| 1 m | [-65, -70] | [-65, -70] | (1 wall) [-75, -80] | -> txPower ≈ -70 |
| | e irrelevant | e irrelevant | e irrelevant | |
| 2 m | [-80, -90] | [-70, -80] | (1 wall) [-75, -80] |  |
| | -> e ≈ [3.3, 6.6] | -> e ≈ [0.0, 3.3] | -> e ≈ [1.7, 3.3] | avg(e) ≈ 3.0 |
| 5 m | [-85, -90] | [-75, -85] | (2 walls) [-85, -90] |  |
| | -> e ≈ [2.1, 2.9] | -> e ≈ [0.7, 2.1] | -> e ≈ [2.1, 2.9] | avg(e) ≈ 2.1 |
| 10 m | [-85, -95] | [-80, -90] | (3 walls) [-90, -100] |  |
| | -> e ≈ [1.5, 2.5] | -> e ≈ [1.0, 2.0] | -> e ≈ [2.0, 3.0] | avg(e) ≈ 2.0 |
| Summary | avg(e) ≈ 3.2 | avg(e) ≈ 1.5 | avg(e) ≈ 2.5 | avg(e) ≈ 2.4 |

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

### Environmental factor estimation

The measured power / txPower is defined as the RSSI value at 1m distance. The above measurements tell us that it must lie somewhere between -65 and -70 if there are no walls in between. Since the RSSI increases (in absolute value) if there are walls in between, a conservative value of -70 has been chosen as the estiamted value for txPower of the Arduino devices.

Next, the formula for calculating the distance to a BLE device depending on the current RSSI, its txPower value, and the environmental factor e looks like this:

> distance = 10 ^ ( (txPower - RSSI) / (10 * e) )

Since in the above experiments we do know the distance but do not know / want to know the environmental factor, we can solve the equation for it:

> e = ( (txPower - RSSI) / log_10(distance) ) / 10

Using this formula, we can now calculate estimates for the environmental factor. These have been added to the table above.

As can be seen, the possible values for the environmental factor fluctuate wildly. The observation of indoors without walls being the most favorable environment is reflected in the environmental factors for it being smaller on average than for the other two environments. The average environmental factors varying so wildly for different distances is a phenomenon I can't explain. Perhaps more test data would be required to gain further insight, but it is debatable whether the related effort would be worth it for this project.

**Summary**: Looking at the averages getting smaller with increased distance, making the 2 m measurements look like outliers, I would recommend using an environmental factor somewhere between 2.0 and 2.5. But the high variation in values tells us that it is difficult to accurately estimate the distance based on RSSI values alone.
