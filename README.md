# Destify_CodeTest

## Warnings and Notes
- This was written and tested in a combination of VSCode and Visual Studio Community 2022, on both Windows and Linux.  Unit tests were written and tested exclusively in VS2022 in Windows.
- Older firefox versions on Windows may not be happy with the certificate Visual Studio creates and uses.  If this is the case, ensure Firefox is up to date, or use a different browser.
- The controllers all follow the same pattern, and the order they appear in the file is more or less chronological in nature.  There are different methodologies for returning values from the services (int vs bool vs exceptions, etc.); this has been intentionally kept inconsistent to demonstrate the evolution of the code (and therefore, my way of thinking) over time.
- Comments are provided where appropriate.  Files such as IXService or XService are not fully documented as they are mostly self-explainatory.
- This project was created using a template.  There may still be a line or two in places that are unrelated to this project that are left over from the template.
- There are still a few edge cases with undesirable behavior.  For example, a PATCH request on an actor can accept a movie update, buit it requires the Movie's name attribute, even though that attribute is ignored.

## Assumptions
* Database calls can be synchronous because we're dealing with a small, local sqlite database.  A real production API should use async db calls.
* Searches are case sensitive