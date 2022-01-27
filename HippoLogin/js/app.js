//this is ran first to while loading the app;

//let strtest=$('#txtEmail').val();

//alert("from app.js"); //simple javascript alert

$(document).on('click','#btnSignIn',function(){ 
    var objNewSessionResponse;
    let blnError = false;
    let strErrorMessage=''; // equals blank so we can use the += operator to add to it

    if(!$('#txtEmail').val()){
        blnError=true;
        strErrorMessage+= '<p>Email/Username is Blank.</p>'
    }
    if(!$('#txtPassword').val()){
        blnError=true;
        strErrorMessage+= '<p>Password is Blank.</p>'
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
        }) 
    }

    $.when(objNewSessionPromise).don(function(){
        if(objNewSessionPromise.Outcome == 'Login Failed'){
            Swal.fire({
                icon:'error',
                title:'Login Failed',
                html: '<p> Review Username and Password </p>'
            })
        } else{
            sessionStorage.setItem('HippoSessionID', objNewSessionPromise.Outcome);
            Swal.fire({
                icon: 'Success',
                title: 'Login Complete',
                html: '<h3> Great Job </h3>'
            })
        }
    })
})