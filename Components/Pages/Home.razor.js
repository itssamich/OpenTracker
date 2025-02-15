function scrollToBottom(elementId){
    console.log(elementId)
    var element = document.getElementById(elementId);
    if (element) {
        element.scrollTop = element.scrollHeight;
    }
};

function test() {
    console.log("test")
}