# To Do #

## Provide a `GetAllAnalysisNotes`
- all contained notes (experiment, (sample set, sample))
- make sure notes derive from an appropriate baseclass, or interface so the report can figure out how to group them.

## Configuration ##

Classes read the YAML configuration on construction- so all instances have the same.

Should split this out so that configuration is a parameter passed into the constructor.

Ie keep the current use case simple, but make it possible to have (useful) multiple instances of these classes.

## Possibly Use Events to propogate analysis note and related changes 

Right now, have several properties in the experimnet class have to be rebuilt on access, in case any of the aggregated sample sets data changes.
Could be a lot more efficient, especially as they're add-only APIs, to just add new items. Events, with the new item as a paramater, would allow this.
