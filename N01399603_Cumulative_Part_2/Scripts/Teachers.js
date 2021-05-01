// AJAX for Teacher Add
// This file is connected to the project via Shared/_Layout.cshtml


function AddTeacher() {

    //goal: send a request which looks like this:
    //POST : http://localhost:62855/api/TeacherData/AddTeacher
    //with POST data of teacher name, employee number, salary, etc.

    var URL = "http://localhost:62855/api/TeacherData/AddTeacher/";

    var rq = new XMLHttpRequest();

    var teacherfname = document.getElementById('teacherfname').value;
    var teacherlname = document.getElementById('teacherlname').value;
    var employeenumber = document.getElementById('employeenumber').value;
    var salary = document.getElementById('salary').value;

    var TeacherData = {
        "teacherfname": teacherfname,
        "teacherlname": teacherlname,
        "employeenumber": employeenumber,
        "salary": salary
    };

    rq.open("POST", URL, true);
    rq.setRequestHeader("Content-Type", "application/json");
    rq.onreadystatechange = function () {
        //ready state should be 4 AND status should be 200
        if (rq.readyState == 4 && rq.status == 200) {
            //request is successful and the request is finished

            //nothing to render, the method returns nothing.

        }

    }
    //POST information sent through the .send() method
    rq.send(JSON.stringify(TeacherData));

}