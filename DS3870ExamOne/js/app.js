// Begin Page Specific Functions

$(document).on('click','#btnAddAccount', function(){
    let strErrorMessage='';
    let blnError = false;

    if(!$('#txtEmail').val()){
        blnError=true;
        strErrorMessage+= '<p>Email/Username is Blank.</p>';
    }else if(!isValidEmail($('#txtEmail').val())){
        
        blnError=true;
        strErrorMessage+='<p>Email/Username is not valid</p>';
    }

    if(!doPasswordsMatch($('#txtPassword').val(),$('#txtVerifyPassword').val())){
        blnError=true;
        strErrorMessage+='<p>Passwords do not match</p>';
    }
        
    

    if(!$('#txtPassword').val()){
        blnError=true;
        strErrorMessage+= '<p>Password is Blank.</p>';
    }else if(!isValidPassword($('#txtPassword').val())){
        
        blnError=true;
        strErrorMessage+='<p>Password is not valid</p>';
    }

    if(blnError == 1){
        Swal.fire({
            icon: 'error',
            title: 'Missing Data',
            html: strErrorMessage
          })
    }else{
        /*
            do not do this in production, this is unprotected API
        */
        var objNewSessionPromise= $.post('https://www.swollenhippo.com/DS3870/Comics/createAccount.php', { strUsername:$('#txtEmail').val(), strPassword:$('#txtPassword').val() }, function(result){
            //console.log(JSON.parse(result).Outcome);
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
                sessionStorage.setItem('HippoTaskID', objNewSessionPromise.Outcome);
                console.log(objNewSessionPromise);
                window.location.href='login.html'; //window.location.replace removes from history
            }
        })
    }
})

$(document).on('click','#btnAddCharacter', function(){
    let strSesID= sessionStorage.getItem('CharacterSession');

    $.getJSON('https://www.swollenhippo.com/DS3870/Comics/verifySession.php', {strSessionID: strSesID}, function(result){
        console.log(result);
        if(result.Outcome == 'Valid Session'){
            let strCName = $('#txtCharacterName').val();
            let strSPower = $('#txtSuperPower').val();
            let strHLocation = $('#txtLocation').val();
            let strHStaturs = $('#selectStatus').val()
            $.post('https://www.swollenhippo.com/DS3870/Comics/addCharacter.php', {strSessionID: strSesID, strName:strCName, strSuperPower:strSPower, strLocation:strHLocation, strStatus:strHStaturs}, function(result){
                let object = JSON.parse(result);
                if(object.Outcome != 'Error'){
                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title:'Task Added.',
                        showConfirmButton: false,
                        timer: 1500
                    })
                    clearCharacterFields()
                    fillCharacterTable();
                } else{
                    Swal.fire({
                        icon: 'error',
                        title: 'Title Not Added',
                        html: '<p> Verify your task data and try again.</p>'
                    })
                }
            })
        } else{
            Swal.fire({
                icon: 'error',
                title: 'Expired Session',
                html: '<p>Oops, appears your session has expired.</p>'
            }).then((result)=>{
                sessionStorage.removeItem('HippoTaskID');
                window.location.href = 'login.html';
            })
        }
    })
})

$(document).on('click','#btnLogin', function(){
    let blnError = false;
    let strErrorMessage = '';
    if(!$('#txtEmail').val()){
        blnError = true;
        strErrorMessage += '<p>Please provide an email address to continue</p>';
    }
    if(!$('#txtPassword').val()){
        blnError = true;
        strErrorMessage += '<p>Please provide your password to continue</p>';
    }
    if(blnError == true){
        Swal.fire({
            icon: 'error',
            title: 'Missing Data',
            html: strErrorMessage
        }) 
    } else{
        $.post('https://www.swollenhippo.com/DS3870/Comics/createSession.php',{strEmail:$('#txtEmail').val(),strPassword:$('#txtPassword').val()},function(result){
        objResult = JSON.parse(result); 
        
        if(objResult.Outcome != 'Login Failed'){
                // set your Session Storage Item here
                sessionStorage.setItem('CharacterSession', objResult.Outcome);
                // then redirect the user to the dashboard
                window.location.href='index.html';
                fillCharacterTable();
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Login Failed',
                    html: '<p>The provided username and password did not match any in our database</p>'
                })
            }
        })
    }
})

$(document).on('click','#btnToggleExisting', function(){
    $('#divCharacters').slideToggle();
})


function clearCreateAccountPage(){
    $('#txtFirstName').val('');
    $('#txtLastName').val('');
    $('#txtEmail').val('');
    $('#txtPassword').val('');
    $('#txtVerifyPassword').val('');
}

function clearCharacterFields(){
    $('#txtCharacterName').val('');
    $('#txtSuperPower').val('');
    $('#txtLocation').val('');
    $('#selectStatus').val('Active').trigger('change');
}

function fillCharacterTable(){
    $('#tblCharacters tbody').empty();
    let strCurrentSessionID = sessionStorage.getItem('CharacterSession');
    $.getJSON('https://www.swollenhippo.com/DS3870/Comics/getCharacters.php',{strSessionID:strCurrentSessionID},function(result){
        $.each(result,function(i,superhero){
            let strTableHTML = '<tr><td>' + superhero.Name + '</td><td>' + superhero.SuperPower + '</td><td>' + superhero.Location + '</td><td>' + superhero.Status + '</td></tr>';
            $('#tblCharacters tbody').append(strTableHTML);
        })
    })
}

// End Page Specific Functions


// Begin Helper Functions
function isValidEmail(strEmailAddress){
    let regEmail = /[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/;
    return regEmail.test(strEmailAddress);
}

function isValidPassword(strPassword){
    let regPassword = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,64}$/;
    return regPassword.test(strPassword);
}

function doPasswordsMatch(strPassword, strVerifyPassword){
    if(strPassword == strVerifyPassword){
        return true;
    } else {
        return false;
    }
}
// End Helper Functions

// Begin Universal Functions
function verifySession(){
    if(sessionStorage.getItem('CharacterSession')){
        let strCurrentSessionID = sessionStorage.getItem('CharacterSession')
        $.getJSON('https://www.swollenhippo.com/DS3870/Test1/verifySession.php', {strSessionID: strCurrentSessionID}, function(result){
            if(result.Outcome != 'Valid Session'){
                return false;
            } else {
                return true;
            }
        })
    } else {
        return false;
    }
}
// End Universal Functions

