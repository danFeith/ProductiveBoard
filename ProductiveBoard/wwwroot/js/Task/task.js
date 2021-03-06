$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);
    if (urlParams.get('typeId') !== null && urlParams.get('typeId') !== '') {
        $('#task-type-id').val(urlParams.get('typeId'));
    }

    if (urlParams.get('statusId') !== null && urlParams.get('statusId') !== '') {
        $('#task-status-id').val(urlParams.get('statusId'));
    }
    if (urlParams.get('sprintId') !== null && urlParams.get('sprintId') !== '') {
        $('#task-sprint-id').val(urlParams.get('sprintId'));
    }

    if (urlParams.get('sprintId') !== null && urlParams.get('sprintId') !== '') {
        $('#task-sprint-id').val(urlParams.get('sprintId'));
    }

    $.get('/Tasks/TasksCountByType', (data) => {
        const mostUsed = { name: '', value: '' };

        let max = 0;

        for (var currData of data) {
            if (currData.value > max) {
                max = currData.value;
                mostUsed.name = currData.name;
                mostUsed.value = currData.value;
            }
        }

        $("#type-select option").each(function () {
            if ($(this).text() === mostUsed.name) {
                $(this).text(`${$(this).text()} - Recommended! ☆`);
            }
        });
    });
});

function fillValues(title, desc, id, statusId, typeId, userId) {
    $('#task-title').val(title);
    $('#task-desc').val(desc);
    $('#task-id').val(id);
    $('#status-id').val(statusId);
    $('#type-id').val(typeId);
    $('#task-status').val(statusId);
    $('#task-type').val(typeId);
    $('#task-user').val(userId);
}

function addTask() {
}

function addSprint() {
    var sprintNameToAdd = document.getElementById("sprintNameInput").value
    $.ajax({
        url: "/Tasks/AddSprint",
        type: "post",
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: sprintNameToAdd,

        success: function (response) {
        },
        error: function (xhr) {
            //Do Something to handle error
        }
    });
    setTimeout("location.reload(true);", 500);

}

function updateTask() {
}

function filter(taskTypeId, taskStatusId) {
    /*$.get(`/Tasks/Filter?typeId=${taskTypeId}&statusId=${taskStatusId}`, (tasks) => {

    });*/
}
