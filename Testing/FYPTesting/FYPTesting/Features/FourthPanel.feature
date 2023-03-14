Feature: Fourth Panel

#Selecting rectangle displays rectangle (data-source contains NAME.svg)
#Selecting triangle displays triangle
#Adjusting each of the edges to each of the colours (FOR BOTH SHAPES)

Background:
	Given the content is refreshed
	Given the tab "Vector Graphics" is open

Scenario Outline: Selecting Object Displays Correctly
	When I select the "<Shape Name>" shape
	Then the "<Shape Name>" shape is displayed
	And "<Num Edges>" amount of edge selection buttons are displayed
	Examples:
	| ID | Shape Name | Num Edges |
	| 1  | Rectangle  | 4         |
	| 2  | Triangle   | 3         |

Scenario Outline: Adjusting Shape Edge Colour Works Correctly
	When I select the "<Shape Name>" shape
	And I select "<Edge>" for edge and "<Colour>" for colour
	And I press the edit button to confirm adjustments
	Then the colour of edge "<Edge>" is changed to "<Colour>"
	Examples: 
	| ID | Shape Name | Edge   | Colour |
	| 1  | Rectangle  | Edge 1 | Black  |
	| 2  | Rectangle  | Edge 1 | Blue   |
	| 3  | Rectangle  | Edge 1 | Green  |
	| 4  | Rectangle  | Edge 1 | Red    |
	| 5  | Rectangle  | Edge 2 | Black  |
	| 6  | Rectangle  | Edge 2 | Blue   |
	| 7  | Rectangle  | Edge 2 | Green  |
	| 8  | Rectangle  | Edge 2 | Red    |
	| 9  | Rectangle  | Edge 3 | Black  |
	| 10 | Rectangle  | Edge 3 | Blue   |
	| 11 | Rectangle  | Edge 3 | Green  |
	| 12 | Rectangle  | Edge 3 | Red    |
	| 13 | Rectangle  | Edge 4 | Black  |
	| 14 | Rectangle  | Edge 4 | Blue   |
	| 15 | Rectangle  | Edge 4 | Green  |
	| 16 | Rectangle  | Edge 4 | Red    |
	| 17 | Triangle   | Edge 1 | Black  |
	| 18 | Triangle   | Edge 1 | Blue   |
	| 19 | Triangle   | Edge 1 | Green  |
	| 20 | Triangle   | Edge 1 | Red    |
	| 21 | Triangle   | Edge 2 | Black  |
	| 22 | Triangle   | Edge 2 | Blue   |
	| 23 | Triangle   | Edge 2 | Green  |
	| 24 | Triangle   | Edge 2 | Red    |
	| 25 | Triangle   | Edge 3 | Black  |
	| 26 | Triangle   | Edge 3 | Blue   |
	| 27 | Triangle   | Edge 3 | Green  |
	| 28 | Triangle   | Edge 3 | Red    |



