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
// "WaitForSeconds" can be changed.
yield WaitForSeconds(20);

Yellow.enabled = true;

Green.enabled = false;
// "WaitForSeconds" can be changed.
yield WaitForSeconds(5);

Red.enabled = true;

Yellow.enabled = false;
// "WaitForSeconds" can be changed.
yield WaitForSeconds (10);

}

 

}





// Script by Charles Carrasco
// YouTube: http://www.youtube.com/user/charlescarrascodev/