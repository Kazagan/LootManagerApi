# Loot Manager API

This is an api that I'm developing to eventually plug into at least a discord both for roll treasure in dnd (specifically 
3rd edition).

__2022 April 5:__  
So far I have the coin endpoint finished, with work started on the coin roller endpoint. I noticed an issue with the coin 
and point that is likely to cause some issues when updating. These issues should fall under the same issue as the
coin roller issue I'm facing. In that I'm currently needing to pass in the full object with the field that 
is being changed. With the desire to have this plug into a discord bot, I want it to be as user friendly as possible,
including only requiring the Id to make changes to an entry. So I need some way of checking the fields
that were passed in, so that anything that is defaulted from not being passed in, is ignored in the update.
This does make it so that you can't insert any values at their default value, but that's maybe something that I 
shouldn't allow anyway. So the problem as it currently exists, is that I need to find a way to check that for each object.
I could create specific methods to do so, but would prefer to find a generic way of doing so, similar to what I created for the 
verification of objects on insert.

So I ended up removing the verification method I had on inserts, and changed it so that I defined it on a case by case basis.
This was due to the fact that the methods I was relying on to do so before were marked as unreliable, and prone to changes. I Then defined the copy 
method, that I wish could be better, but currently can't think of a way to implement what I want in another way.