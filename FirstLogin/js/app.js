$(document).on('click','#btnSignIn',function(){ 
    var objNewSessionResponse;
    let blnError = false;
    let strErrorMessage=''; // equals blank so we can use the += operator to add to it

    if(!$('#txtEmail').val()){
        blnError=true;
        strErrorMessage+= '<p>Email/Username is Blank.</p>'
    }else if(!validateUsername($('#txtEmail').val())){
        console.log("Bad Username");
        blnError=true;
        strErrorMessage+='<p>Email/Username is not valid</p>'
    }


    if(!$('#txtPassword').val()){
        blnError=true;
        strErrorMessage+= '<p>Password is Blank.</p>'
    }else if(!validatePassword($('#txtPassword').val())){
        console.log('Bad Password');
        blnError=true;
        strErrorMessage+='<p>Password is not valid</p>'
    }

    //error handing with truthy stats
    if(blnError == 1){
        Swal.fire({
            icon: 'error',
            title: 'Missing Data',
            html: strErrorMessage
          })
    }else{
        /*
            do not do this in production this is unprotected API
        */
        var objNewSessionPromise= $.post('https:www.swollenhippo.com/DS3870/newSession.php', { strUsername:$('#txtEmail').val(), strPassword:$('#txtPassword').val() }, function(result){
            console.log(JSON.parse(result).Outcome);
            objNewSessionPromise = JSON.parse(result);
        }) 

        $.when(objNewSessionPromise).done(function(){
            if(objNewSessionPromise.Outcome == 'Login Failed'){
                Swal.fire({
                    icon:'error',
                    title:'Login Failed',
                    html: '<p> Review Username and Password </p>'
                })
            } else{
                sessionStorage.setItem('HippoSessionID', objNewSessionPromise.Outcome);
                window.location.href='index.html'; //window.location.replace removes from history
            }
        })
    }

    
})

function validateUsername(strUsername){
    let reg = /\S+@\S+\.\S+/;
    return reg.test(strUsername);
}

function validatePassword(strPassword){
    let reg= /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/;
    return reg.test(strPassword);
}