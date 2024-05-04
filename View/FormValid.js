const inputs = document.querySelectorAll("input:not(:is([type='submit'], [type='radio'], [type='file']))")
             , form = document.getElementById("form1");

inputs.forEach((input) => {

    input.oninput = () => {

        let pattern = new RegExp(`${input.dataset.pattern}`);  // regex pattern

        if (input.value != "") {  // input empty  

            if (pattern.test(input.value)) {
                input.classList.add("valid");
                input.classList.remove("invalid");
            }  // input valid
            else {
                input.classList.add("invalid");
                input.classList.remove("valid");
            } // input invalid

        }

        else {
            input.classList.remove("invalid");
            input.classList.remove("valid");
        }

    };

});


form.onsubmit = () => {
    return document.querySelectorAll("input.invalid").length === 0;
};


