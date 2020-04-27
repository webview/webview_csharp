var sendAnswer = function() {
    evalTest("What", "is", "the", "answer?")
        .then((x) => document.getElementById('answer').innerHTML = x.result);
}