# Destify_CodeTest

## Assumptions
* Database calls can be synchronous because we're dealing with a small, local sqlite database.  A real production API should use async db calls.
* The interpretation of "Must allow you to do partial searches for Movies or Actors, based on name; View all Movies an Actor has been in, and list all Actors in a given a Movie" is that viewing all actors of a movie and all movies an actor has been in will ONLY be done when searching by name for movies or actors.  For example, searching for a movie by id will **not** include actor information.  Additionally, the average movie rating will be included whenever a movie is returned.
