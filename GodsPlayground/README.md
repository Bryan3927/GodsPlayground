# Installation Instructions

locate and run "God's Playground.exe" in the base directory

# Known Major Bugs

- Technically speaking, speedboost v1/v2 doesn't work as intended (i.e. only the animation gets faster, but the rate
at which animals make decisions is the same). This is fine in terms of gameplay, however, because this trait is not
necessary to win and would only help the player win more quickly.
- Lag; There is considerable lag due to particle effects and #of calc/animal when # of animals nears 200
- Animals may get stuck on an action and appear to stop moving (often breaking out of it later)
- Bunnies with smart eating trait may get stuck eating a single piece of grass over and over
- Removal of Entities when they die (via eating, starving or any other way) can very rarely throw an error if
removal occurs from two sources (i.e. if two foxes ate the same bunny at the exact same time)