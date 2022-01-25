//this is ran first to while loading the app;

//let strtest=$('#txtEmail').val();

//alert("from app.js"); //simple javascript alert

$(document).on('click','#btnSignIn',function(){
    console.log('btnSignIn Clicked'); // console.log is a debug feature
    let strUsername= ' '; //let local scoped variable

    strUsername=$('#txtEmail').val(); //setting value of strUSername
    console.log('Set value of strUsername = ' + strUsername); //concatenation?
    localStorage.setItem('Username', strUsername);
    console.log('Set localStorage Username');
    }
);