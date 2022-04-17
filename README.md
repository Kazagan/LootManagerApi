# Loot Manager API

This is an api that I'm developing to eventually plug into at least a discord bot for roll treasure in dnd (specifically 
3rd edition). Some of the design choices come from wanting it to be user friendly to use from discord bot commands.

Getting End to End tests up and running has been more difficult than anticipated. A large part in due to wanting to ensure that the tests clean up after themselves. Attempting to do the cleanup at the end of the tests was causing issues so tests are currently cleaning up after themselves. I also took the time to make the end points utilize async/await, something I originally wasn't going to do until later. 
