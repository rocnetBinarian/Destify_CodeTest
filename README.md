# Destify_CodeTest

## Warnings
- Older firefox versions on Windows may not be happy with the certificate Visual Studio creates and uses.  If this is the case, ensure Firefox is up to date, or use a different browser.

## Assumptions
* Database calls can be synchronous because we're dealing with a small, local sqlite database.  A real production API should use async db calls.
* Searches are case sensitive