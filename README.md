# Destify_CodeTest

## Warnings and Notes
- Older firefox versions on Windows may not be happy with the certificate Visual Studio creates and uses.  If this is the case, ensure Firefox is up to date, or use a different browser.
- The controllers all follow the same pattern, and the order they appear in the file is more or less chronological in nature.  There are different methodologies for returning values from the services (int vs bool vs exceptions, etc.); this has been intentionally kept inconsistent to demonstrate the evolution of the code (and therefore, my way of thinking) over time.
- Comments are provided where appropriate.  Files such as IXService or XService are not fully documented as they are mostly self-explainatory.

## Assumptions
* Database calls can be synchronous because we're dealing with a small, local sqlite database.  A real production API should use async db calls.
* Searches are case sensitive