# AstralZeroLoR

Project Name: Stacked Deck

Link: 

Stacked Deck is a third party app that is used to generate a Randomized Deck for users to use in Legends of Runeterra.

Process:

Upon loading this app, Users are able to login, create a new account, or play as a guest
	By logging in users will be able to: 
		Update the amount of each card they have.
			Allows the app to generate a deck immediately usable.
 		Save and Load previously generated decks.

As a guest, uses will not be able to update card amounts nor will they be able to save and load decks.
    
IMPORTANT NOTE: When prompted for Username and Password, this is NOT your Legends of Runeterra login information.
    			Please use an arbitrary Username and Password for registration, one that does not contain any personal information.
    			You may choose to use the same login information as Legends of Runeterra but do so AT YOUR OWN RISK as it is stored on a database for later retrieval.
Deck Generation: 
	Simple: 
		Press the generate button to generate a completely random deck (2 random regions and a random amount of cards per region)
	Advanced: 
		Press the Advanced button in the top right corner to set your own queries. 
		Upon entering the desired regions press Generate to generate the deck. 
		Users are also able to press the Randomize button to have completely random queries.
        
Additional Features: 
	View Deck:
		Allows the user to see the cards in the deck they generated allow with how many copies of each card are in the deck.
	Copy to Clipboard: 
		The clipboard button allows players to easily copy the deck code and paste it into LoR client.
	(THE FOLLOWING FEATURES ARE FOR LOGGED IN USERS ONLY)
	Update: 
		The update button in the bottom left hand corner allows users to search for a certain card by its name and type.
		Click on a certain card to zoom in and press the arrows on either side to bring up panels: 
			Left: 
				Shows the user how many copies of that card they currently have.
			Right: (Will only be available for logged in users)
				Allows the user to input how many copies of that card they have. (input must be between 0 and 3)
	Load: 
		Brings up decks that the user had previously saved. (saves up to 3 decks)
		Displays the deck name, regions, and number of cards per region in the deck.
	Save: 
		Save the currently generated deck onto a certain slot.
		Allows users to name the deck for better identification.
		*WARNING: will override the deck currently in that slot*


Languages used: 
	Server:
		Node.js, Express, MongoDB, Hosted on Heroku
	Database: 
		MySQL
	App:
		Unity, C#

Created by: 
	Daniel Qi, Chef BoyarQi, Computer Game Science/Computer Science, Junior, UCI
	Timothy Quach, tworii, Computer Game Science, Junior, UCI
	Jonathan Chhou, Ripof, Data Science, Junior, UCI

Thanks for using our application!



Stacked Deck isn't endorsed by Riot Games and doesn't reflect the views or opinions of Riot Games or anyone officially involved in producing or managing Legends of Runeterra. Legends of Runeterra and Riot Games are trademarks or registered trademarks of Riot Games, Inc. Legends of Runeterra © Riot Games, Inc.