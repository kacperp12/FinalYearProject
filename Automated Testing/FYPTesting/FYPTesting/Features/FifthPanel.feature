Feature: Fifth Panel
# Feature file containing tests for the fifth panel of the FT Optix project

Background:
	Given the content is refreshed
	Given the tab "Reusable Graphics" is open

Scenario Outline: Selecting Input Type Displays Correct Graphics
	When I select "<Input Type>" from the selection of input types
	Then the label "<Input Type>" is displayed above the textbox
	Examples: 
	| ID | Input Type |
	| 1  | Eircode    |
	| 2  | Email      |
	| 3  | Number     |

Scenario Outline: Successful Submission Displays a New Entry in the Gridbox
	When I select "<Input Type>" from the selection of input types
	* I input "<Input String>" into the textbox
	* I press the submit button
	* I acknowledge the submission
	Then a new entry of type "<Input Type>" is placed at the top of the gridbox
	Examples: 
	| ID | Input Type | Input String     |
	| 1  | Eircode    | H91 Y8W7         |
	| 2  | Email      | kacper@gmail.com |
	| 3  | Number     | 0851230377       |

Scenario Outline: Input Validation for Selection of Input Types (Valid Entries)
	When I select "<Input Type>" from the selection of input types
	* I input "<Input String>" into the textbox
	* I press the submit button
	Then I am prompted that my submission is successful
	Examples: 
	| ID | Input Type | Input String                 | Comment                            |
	| 1  | Eircode    | D24 FT12                     | Standard Eircode                   |
	| 2  | Eircode    | X99 9999                     | Edge-case Eircode                  |
	| 3  | Email      | k@k.com                      | Shortest Possible Input            |
	| 4  | Email      | kacperkacperkac@yahooooo.com | Longest Possible Input             |
	| 5  | Email      | k@yahoo.com                  | Shortest Possible Username Only    |
	| 6  | Email      | kacper@y.com                 | Shortest Possible Domain Name Only |
	| 7  | Number     | 0851230377                   | Standard Number                    |
	| 8  | Number     | 1111111111                   | Edge-case Number                   |

Scenario Outline: Input Validation for Selection of Input Types (Invalid Entries)
	When I select "<Input Type>" from the selection of input types
	* I input "<Input String>" into the textbox
	Then the submit button is disabled 
	Examples: 
	| ID | Input Type | Input String                  | Comment                                              |
	| 1  | Eircode    | H91Y7W8                       | No space between routing keys and unique identifiers |
	| 2  | Eircode    | H91                           | Invalid number of chars                              |
	| 3  | Eircode    | []+-_@                        | Invalid input                                        |
	| 4  | Eircode    | H91 111111                    | Too many chars                                       |
	| 5  | Eircode    |                               | No chars                                             |
	| 6  | Eircode    | H91 Y7W‰                      | Unicode character (per mille sign)                   |
	| 7  | Email      | kacperkacperkacp@yahooooo.com | Username part too long                               |
	| 8  | Email      | kacperkacperkac@yahoooooo.com | Domain name part too long                            |
	| 9  | Email      | @yahoo.com                    | No username                                          |
	| 10 | Email      | kacper@.com                   | No domain name                                       |
	| 11 | Email      | kacper yahoo.com              | Space instead of "at" sign                           |
	| 12 | Email      |                               | No input                                             |
	| 13 | Email      | kacper@yahoo                  | No ".com"                                            |
	| 14 | Number     |                               | No input                                             |
	| 15 | Number     | letters                       | Letters not allowed                                  |
	| 16 | Number     | 111                           | Input too short                                      |
	| 17 | Number     | 111111111111111               | Input too long                                       |

Scenario: Incorrect Input Displays Error Message
	When I select "Eircode" from the selection of input types
	* I input "test" into the textbox
	Then an error message is displayed
	* the submit button is disabled