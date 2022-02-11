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
        var objNewSessionPromise= $.post('https://www.swollenhippo.com/DS3870/Tasks/newSession.php', { strUsername:$('#txtEmail').val(), strPassword:$('#txtPassword').val() }, function(result){
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
                window.location.href='index.html'; //window.location.replace removes from history
            }
        })
    }

    
})

//for creating new users
$(document).on('click', '#btnCreate', function(){
    var newAccount = $.post('https://swollenhippo.com/DS3870/Tasks/newAccount.php', {strUsername:$('#txtEmail').val(),strPassword:$('#txtPassword').val()}, function(result){
    console.log(JSON.parse(result).Outcome);
    newAccount = JSON.parse(result);
    })

    $.when(newAccount).done(function(){
        if(newAccount.Outcome == 'Error - User Not Created'){
            Swal.fire({
                icon:'error',
                title:'User Account not Created',
                html: '<p> try again </p>'
            }).then((result)=>{window.location.href='login.html';})
        }
        if(newAccount.Outcome == 'User Already Exists'){
            Swal.fire({
                icon:'error',
                title:'User Already Exists',
                html: '<p> Login with your existing credentials </p>'
            }).then((result)=>{window.location.href='login.html';})
            
        }
        else{
            Swal.fire({
                icon:'success',
                title:'Account Created Successfully',
                html: '<p> Please login with your credentials </p>'
            }).then((result)=>{window.location.href='login.html';})
            
        }
    })
})

//adding a task
$(document).on('click', '#btnAddTask', function(result){
    let strSesID= sessionStorage.getItem('HippoTaskID');

    $.getJSON('https://www.swollenhippo.com/DS3870/Tasks/verifySession.php', {strSessionID: strSesID}, function(result){
        console.log(result);
        if(result.Outcome == 'Valid Session'){
            let strTaskNames = $('#txtTaskName').val();
            let strLocations = $('#txtLocation').val();
            let strDate = $('#txtDueDate').val();
            let strNote = $('#txtNotes').val();
            $.post('https://www.swollenhippo.com/DS3870/Tasks/newTask.php', {strSessionID: strSesID, strLocation:strLocations, strTaskName: strTaskNames, strNotes: strNote}, function(result){
                let object = JSON.parse(result);
                if(object.Outcome != 'Error'){
                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title:'Task Added.',
                        showConfirmButton: false,
                        timer: 1500
                    })
                    $('#txtTaskName').val('');
                    $('#txtLocation').val('');
                    $('#txtDueDate').val('');
                    $('#txtNotes').val('');
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

function fillTasks(){
    $.getJSON('https://www.swollenhippo.com/DS3870/Tasks/getTasks.php', {strSessionID:sessionStorage.getItem('HippoTaskID')}, function(result){
        console.log(result); 
        $('#tblTasks tbody').empty();
        $.each(result,function(i,tblTasks){
            if(tblTasks.Status == 'ACTIVE'){
                console.log(tblTasks);
                
                let strTableHTML='<tr> <td>' + tblTasks.strTaskName + ' </td><td> '+ tblTasks.strLocation + ' </td><td> '+ tblTasks.strDate +' </td><td> '+ tblTasks.strNotes +' </td><td><button class="btn btn-success btnTaskComplete"data-taskid="'+ tblTasks.TaskID+'">Complete</button>  <button class="btn btn-danger btnTaskDelete"data-taskid="'+ tblTasks.TaskID+'">Delete</button></td> </tr>';
                $('#tblTasks tbody').append(strTableHTML); 
            }
            //$('#tblTasks').DataTable();
        })
    })
}

$(document).on('click', '#toggleAdd', function(){
    $('#divAddNewTask').slideToggle();
    //$('#').slideUp();
})

$(document).on('click', '.btnTaskComplete', function(){
    console.log('complete' + $(this).attr('data-TaskID'));
    let strTaskID=$(this).attr('data-TaskID');
    let sessionID=sessionStorage.getTIem('HippoTaskID');

    $.post('https://swollenhippo.com/DS3870/Tasks/markTaskComplete.php',{strSessionID: sessionID , strTaskID: strTaskID}, function(){
        console.log(result);
        fillTasks();
    })
})

$(document).on('click', '.btnTaskDelete', function(){
    console.log('delete' +$(this).attr('data-TaskID'));
    let strTaskID=$(this).attr('data-TaskID');
    let sessionID=sessionStorage.getTIem('HippoTaskID');

    $.post('https://swollenhippo.com/DS3870/Tasks/deleteTask.php',{strSessionID: sessionID , strTaskID: strTaskID}, function(){
        console.log(result);
        fillTasks();
    })
})

function validateUsername(strUsername){
    let reg = /\S+@\S+\.\S+/;
    return reg.test(strUsername);
}

function validatePassword(strPassword){
    let reg= /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/;
    return reg.test(strPassword);
}