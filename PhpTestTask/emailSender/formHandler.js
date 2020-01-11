function processForm() {
    let emailRecipient = document.getElementsByName('EmailRecipient')[0].value;
    let emailMessage = document.getElementsByName('Message')[0].value;

    if (checkEmail(emailRecipient)) {
        if (emailMessage)
        {
            let form = document.getElementById('emailForm');
            let formData = new FormData(form);
            postRequest('main.php', formData);

            return false;
        }

        showMessage('Warning: You have entered an empty message!');
        return false;
    }
    else {
        showMessage('Warning: You have entered an invalid email address!');
        return false;
    }
}

function postRequest(url, data) {
    fetch(url, {
        method: 'POST',
        mode: 'no-cors',
        cache: 'no-cache',
        body: data
    })
        .then(handleErrors)
        .then(response => {
            response.text()
                .then(responseText => showMessage(responseText))
        })
        .catch(error => {
            error.text()
                .then(errorText => showMessage(errorText))
        });
}

function handleErrors(response) {
    if (!response.ok) {
        throw Error(response.statusText);
    }

    return response;
}

function showMessage(text) {
    alert(text);
}

function checkEmail(email) {
    let regex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    return regex.test(email);
}
