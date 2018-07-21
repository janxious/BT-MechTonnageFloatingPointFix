# BT-MechTonnageFloatingPointFixer

Make the check for underweight and overweight mechs in mech lab use less precision. Floating point math around the check can cause false postives in mech validation.

## Settings

Setting | Type | Default | Description
--- | --- | --- | ---
`epsilon` | `float` | `0.0001` | Any difference between max chassis tonnage and current tonnage less than this number is considered 0
`debug` | `bool` | `false` | enable debug logging

## LICENSE

[MIT](LICENSE) licensed.