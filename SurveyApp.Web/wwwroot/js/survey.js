// Variable to store question and answer data
let questionAndAnswerData = [];

const progress = (value) => {
    document.getElementsByClassName('progress-bar')[0].style.width = `${value}%`;
}

let step = document.getElementsByClassName('step');
let prevBtn = document.getElementById('prev-btn');
let nextBtn = document.getElementById('next-btn');
let submitBtn = document.getElementById('submit-btn');
let form = document.getElementsByTagName('form')[0];
let preloader = document.getElementById('preloader-wrapper');
let bodyElement = document.querySelector('body');
let succcessDiv = document.getElementById('success');
let surveyId = document.getElementById('SurveyId');

form.onsubmit = () => { return false }

let current_step = 0;
let stepCount = 6
step[current_step].classList.add('d-block');
if (current_step == 0) {
    prevBtn.classList.add('d-none');
    submitBtn.classList.add('d-none');
    nextBtn.classList.add('d-inline-block');
}

nextBtn.addEventListener('click', () => {
    current_step++;
    let previous_step = current_step - 1;
    if ((current_step > 0) && (current_step <= stepCount)) {
        prevBtn.classList.remove('d-none');
        prevBtn.classList.add('d-inline-block');
        step[current_step].classList.remove('d-none');
        step[current_step].classList.add('d-block');
        step[previous_step].classList.remove('d-block');
        step[previous_step].classList.add('d-none');
        if (current_step == stepCount) {
            submitBtn.classList.remove('d-none');
            submitBtn.classList.add('d-inline-block');
            nextBtn.classList.remove('d-inline-block');
            nextBtn.classList.add('d-none');
        }

        // Iterate through the options of the current question
        let questionOptions = step[previous_step].querySelectorAll('.question__input');
        let selectedAnswerId = null;

        // Find the selected answer (assuming only one can be selected for a question)
        for (let option of questionOptions) {
            if (option.checked) {
                selectedAnswerId = option.getAttribute('data-answerid');
                break;
            }
        }

        // Store question and answer data
        let questionId = step[previous_step].querySelector('.questionId').getAttribute('data-questionid');
        questionAndAnswerData.push({
            questionId: questionId,
            answerId: selectedAnswerId,
            surveyId: surveyId
        });

    } else {
        if (current_step > stepCount) {
            form.onsubmit = () => { return true }
        }
    }
    progress((100 / stepCount) * current_step);
});


prevBtn.addEventListener('click', () => {
    if (current_step > 0) {
        current_step--;
        let previous_step = current_step + 1;
        prevBtn.classList.add('d-none');
        prevBtn.classList.add('d-inline-block');
        step[current_step].classList.remove('d-none');
        step[current_step].classList.add('d-block')
        step[previous_step].classList.remove('d-block');
        step[previous_step].classList.add('d-none');

        // Iterate through the options of the current question
        let questionOptions = step[previous_step].querySelectorAll('.question__input');
        let selectedAnswerId = null;

        // Find the selected answer (assuming only one can be selected for a question)
        for (let option of questionOptions) {
            if (option.checked) {
                selectedAnswerId = option.getAttribute('data-answerid');
                break;
            }
        }

        // Update the answer in the questionAndAnswerData array
        let questionId = step[previous_step].querySelector('.questionId').getAttribute('data-questionid');
        let existingDataIndex = questionAndAnswerData.findIndex(item => item.questionId === questionId);

        if (existingDataIndex !== -1) {
            questionAndAnswerData[existingDataIndex].answerId = selectedAnswerId;
        }

        if (current_step < stepCount) {
            submitBtn.classList.remove('d-inline-block');
            submitBtn.classList.add('d-none');
            nextBtn.classList.remove('d-none');
            nextBtn.classList.add('d-inline-block');
            prevBtn.classList.remove('d-none');
            prevBtn.classList.add('d-inline-block');
        }
    }

    if (current_step == 0) {
        prevBtn.classList.remove('d-inline-block');
        prevBtn.classList.add('d-none');
    }
    progress((100 / stepCount) * current_step);
});


submitBtn.addEventListener('click', () => {

    preloader.classList.add('d-block');

    const timer = ms => new Promise(res => setTimeout(res, ms));

    timer(3000)
        .then(() => {
            bodyElement.classList.add('loaded');
        }).then(() => {
            step[stepCount].classList.remove('d-block');
            step[stepCount].classList.add('d-none');
            prevBtn.classList.remove('d-inline-block');
            prevBtn.classList.add('d-none');
            submitBtn.classList.remove('d-inline-block');
            submitBtn.classList.add('d-none');
            succcessDiv.classList.remove('d-none');
            succcessDiv.classList.add('d-block');
        });

    $.ajax({
        url: '/Survey/AnswerAjaxPost',
        type: 'POST',

        data: { questionAndAnswerData: questionAndAnswerData },
        //beforeSend: function () {
        //    $.blockUI({
        //        css: {
        //            border: 'none',
        //            padding: '15px',
        //            backgroundColor: '#000',
        //            '-webkit-border-radius': '10px',
        //            '-moz-border-radius': '10px',
        //            opacity: .5,
        //            color: '#fff'
        //        }
        //    });
        //},
        success: function (data) {
            //$.unblockUI();
            //if (data.isSuccess == true) {
            //    // Handle success
            //    $.blockUI({
            //        message: 'Thank you! survey saved successfully',
            //        fadeIn: 700,
            //        fadeOut: 700,
            //        timeout: 10000,
            //        showOverlay: false,
            //        centerY: false,
            //        css: {
            //            width: '350px',
            //            top: '10px',
            //            left: '',
            //            right: '10px',
            //            border: 'none',
            //            padding: '5px',
            //            backgroundColor: '#000',
            //            '-webkit-border-radius': '10px',
            //            '-moz-border-radius': '10px',
            //            opacity: .6,
            //            color: '#fff'
            //        }
            //    });
            //});
        }
    });
});

