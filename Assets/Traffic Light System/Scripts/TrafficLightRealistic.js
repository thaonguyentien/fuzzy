// Script by Charles Carrasco
// YouTube: http://www.youtube.com/user/charlescarrascodev/
// Notes:
// "WaitForSeconds" can be changed to change time the light stays on.





var Red : Light;

var Green : Light;

var Yellow : Light;

 

function Start()

{

 

Yellow.enabled = false;

 

while(true)

{

Green.enabled = true;

Red.enabled = false;

yield WaitForSeconds(30);

Yellow.enabled = true;

Green.enabled = false;

yield WaitForSeconds(4);

Red.enabled = true;

Yellow.enabled = false;

yield WaitForSeconds (30);

}

 

}





// Script by Charles Carrasco
// YouTube: http://www.youtube.com/user/charlescarrascodev/