var _wordIndex = 0;
var _words = [];
var _newCarouselhtml = '';
var _webUrl = '/';//window.location.href;
var _numberOfImages = 20;
var _allWords = '';
var _targetWordArr = '';
var _recentWords = new Array();
var _currentWordsParts = new Array();
var _recentWordsMeanings = new Array();
var _wordIndexToPlay = 0;
var _wordRepeationThresholdForLearning = 13;
var _numberOfAutomaticRepetitionOfCurrentWord = 0;
var _playWordQueueInterval = 13000;
var _wordReviewStartTime = new Date();
var _wordReviewEndTime = new Date();
var _tokenKey = 'accessToken';
var _lock = 1;
var _intervalHandle = 0;
var _xAxisData = new Array();
var _yAxisData = new Array();

$(function () {
    initialize();
    //loadResource();

});
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function initialize() {

    if (_lock == 1) {
        _lock = 0;
        var sec = _playWordQueueInterval / 1000;
        $('#txtWordRepeationInterval').val(sec.toString());
        $('#lblNumberOfAutomaticRepetitions').val(_numberOfAutomaticRepetitionOfCurrentWord.toString());
        $('#lblWordRepeationThresholdForLearning').val(_wordRepeationThresholdForLearning.toString());
        $('#btnSignup').click(signup);
        $('.dropdown-toggle').dropdown();
        var resource = null;
        $('li').on('click', setWordReviewStrategy);
        'use strict';
        //clearInterval();

        callController();

        $("#txtMeaning").on("input propertychange paste", meaningBoxKeyDown);
        $("#txtWord").on("input propertychange paste", meaningBoxKeyDown);
        $("#txtMeaning").on("keypress kyup keydown", meaningBoxKeyDown);
        $("#txtWord").on("keypress kyup keydown", meaningBoxKeyDown);

        $("#btnUndo").click(undo);

        $('#importFile').slideUp();
        //$("#vidMain").on("play", videoPlayingHandler);

        $('#successAlert').slideDown();

        $('#btnAddWord').click(showAddWordDiv);

        $('#btnImportFile').click(importFile);

        $('#btnCreateGraph').click(createGraph);

        //$('#btnBatchImport').click(batchImport);

        $('#btnCreateNewWord').click(takeDecisionForNewInformation);
        $('#btnUpdateWord').click(takeDecisionForNewInformation);
        $('#btnDeleteWord').click(deleteWord);
        $('#btnSetAmbiguous').click(setAmbiguous);
        $('#btnLogin').click(login);
        $('#btnChangePassword').click(changePassword);


        $('#ancLeftArrowCarousel').click(loadOrderedWord);
        $('#ancRightArrowCarousel').click(loadOrderedWord);

        $('#btnKnow').click(updateWordStatus);
        $('#btnDontKnow').click(updateWordStatus);

        $('#btnCloseAddWord').click(closeAddWordPanel);

        $('#btnLoadOrdNetData').click(loadOrdNetData);

        $('#images-frame').empty();


        initializeSliders();
        initializeTooltips();
    }

}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function callController() {

    if (window.location.href.toString().indexOf("index.html") >= 0) {
        loadWords();
        _intervalHandle = setInterval(playWordQueue, _playWordQueueInterval)
        addImageBoxes();
        showChart();
    }
    else
    {
        // if(!_isLoaded )
            // window.location.href = _webUrl+"login.html"
        //var token = sessionStorage.getItem(_tokenKey);
        //var headers = {};
        //if (token) {
        //    headers.Authorization = 'Bearer ' + token;
        //}

    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function addImageBoxes() {

    for (var i = 1; i <= _numberOfImages; i++) {

        var theSrc = '/images/noImage.jpg';
        $('#images-frame').append('<div class="imageBox">' +
            '<img id="img' + i.toString() + '" src=' + theSrc + ' class="wordImage" alt="No image available" data-toggle="No image available" />' +
          '</div>'
        );

    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function undo(e) {
    e.preventDefault();

    var id = _words[_wordIndex].Id;
    var oldWord = getWordLocally(id);

    _words[_wordIndex].TargetWord = oldWord.TargetWord;
    _words[_wordIndex].WrittenByMe = oldWord.WrittenByMe;
    _words[_wordIndex].Meaning = oldWord.Meaning;

    updateWord(e);
    showWord(_wordIndex);
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function changePassword() {

    var oldPassword = $("#txtCPOldPassword").val();
    var newPassword = $("#txtCPNewPassword").val();
    var confirmNewPassword = $("#txtCPConfirmNewPassword").val();

    var changePassData = { "OldPassword": oldPassword, "NewPassword": newPassword, "ConfirmPassword": confirmNewPassword }
    var token = sessionStorage.getItem(_tokenKey);
    var headers = {};
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }

    $.ajax({
        url: _webUrl + 'api/Account/ChangePassword',
        type: 'post',
        async: false,
        dataType: "json",
        data: changePassData,
        scriptCharset: "utf-8",
        contentType: "application/x-www-form-urlencoded;charset=utf-8",
        encoding: "UTF-8",
        headers: headers,
        success: function (e1) {
            alert("Change Password finished successfully");
        },
        error: function (e2) {
            if (e2.status != 200)
                alert('ChangePasswordError ' + e2.responseText.toString());
            else
                alert("Change Password finished successfully with status Code 200");
        }
    });

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function signup() {

    var email = $("#txtEmailSignup").val();
    var password = $("#txtPasswordSignup").val();

    $.ajax({
        url: _webUrl + 'api/Account/Register',
        type: 'post',
        async: false,
        dataType: "json",
        data: { "Email": email, "Password": password, "ConfirmPassword": password },
        scriptCharset: "utf-8",
        contentType: "application/x-www-form-urlencoded;charset=utf-8",
        encoding: "UTF-8",
        success: function (e1) {
            alert("Signup finished successfully");
            loginFunction(email, password);
        },
        error: function (e2) {
            if (e2.status != 200)
                alert('Signup ' + e2.toString());
        }
    });

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function login() {


    var email = $("#txtEmailLogin").val();
    var password = $("#txtPasswordLogin").val();

    var loginResult = null;
    loginFunction(email, password);
    var token = sessionStorage.getItem(_tokenKey);
    if (token) {
        loginResult = true;
    }
    if (loginResult) {

        window.location.href = "/index.html";



    }
    else
        alert("unsuccessful login");
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function loginFunction(email, password) {



    var loginData = {
        grant_type: 'password',
        username: email,
        password: password
    };

    $.ajax({
        type: 'POST',
        url: '/Token',
        async: false,
        data: loginData,
        success: function (data) {
            var d1 = new Date();
            // Cache the access token in session storage.
            sessionStorage.setItem(_tokenKey, data.access_token);
            //$('#btnCloseLoginModal').click();
            //$('#btnCloseLoginModal').modal('hide');
            //$('#paneLogin').modal('hide');
            //$('#btnModalDialog').modal('hide');
            //$('#btnCloseModalTop').click();
            //$('#loginModal').click();
            //$('#loginModal').modal('hide');

            //if ($('#liChangePassword') == null || $('#liChangePassword') == undefined) {
            //    var itemText = '<li id="liChangePassword" style="display:none;">' +
            //                        '<a href="#" id="ancChangePassword" data-toggle="modal" data-target="#changePasswordModal"><span class="glyphicon glyphicon-user"></span> Change Password</a>' +
            //                    '</li>';
            //    $('#ulMenuItem').append(itemText);
            //    alert(itemText);
            ////}

            return true;
        },
        error: function (e) {
            var d1 = new Date();
            //alert('login error' + d1.toString());
            //$('#loginModal').modal('show');
            return false;
        }
    });


}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function initializeSliders() {

    $("#divMain > span").each(function () {
        // read initial values from markup and remove that
        var value = parseInt($(this).text(), 10);
        $(this).empty().slider({
            value: value,
            range: "min",
            animate: true,
            orientation: "horizontal"
        });
    });


}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function setWordReviewStrategy() {

    $('.dropdown-toggle').map(function (index, domElement) {
        //alert($(domElement)[0].id);
        if ($(domElement)[0].id == "ancWordReviewStrategy")
            $(domElement).html($(this).html() + '<span class="caret"></span>')
    });


}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function playWordQueue() {

    //showWord(_wordIndex);
    //var sec = parseInt($('#txtWordRepeationInterval').val());
    //if (_playWordQueueInterval != sec * 1000) {
    //    _playWordQueueInterval = sec * 1000;
    //    clearInterval(_intervalHandle)
    //    _intervalHandle = setInterval(playWordQueue, _playWordQueueInterval);
    //}

    _wordRepeationThresholdForLearning = parseInt($('#lblWordRepeationThresholdForLearning').val());
    var pronounceWholeWord = $('#chkPronounceWholeWord').is(":checked");
    var pronounceWordAndMeaning = $('#chkPronounceWordAndMeaning').is(":checked");

    var wordText = _words[_wordIndex].Id.toString();
    if (pronounceWholeWord) {

        if (_currentWordsParts.length > 1 ) {
            if (pronounceWordAndMeaning) {


                try {


                    if (_wordIndexToPlay % 2 == 0)
                        playText(wordText);
                    else {
                        playTextByLang(wordText, "en");

                    }
                } catch (e) {
                    var i = e;
                }
            }
            else {
                playText(wordText);
            }

         
        }


    
    else {
        var currentPart = _currentWordsParts[0];
        var word = { TargetWord: currentPart };
        var d = new Date();

        showImagesForWord(word);
        playText(currentPart);


        }
        _numberOfAutomaticRepetitionOfCurrentWord++;
        if (_numberOfAutomaticRepetitionOfCurrentWord >= _wordRepeationThresholdForLearning) {
            _numberOfAutomaticRepetitionOfCurrentWord = 0;
            updateWordStatusToSpecificResult(true, _words[_wordIndex].Id);
            goToNextWord();

        }
        $('#lblNumberOfAutomaticRepetitions').val(_numberOfAutomaticRepetitionOfCurrentWord.toString());
    }
   
    _wordIndexToPlay = (_wordIndexToPlay + 1) % _currentWordsParts.length;


}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function wordExist(text) {

    var word = getWordByTargetWord(text)
    if (word == '' || word == null)
        return false;

    return true;

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function meaningBoxKeyDown(e) {

    var text = $("#txtWord").val();
    var wordExists = wordExist(text);

    if (this.id == 'txtWord') {
        $("#btnCreateNewWord").prop('disabled', false);
        $("#btnCreateNewWord").attr("class", "btn btn-default")

        loadAllWords(text);

        if (wordExists) {
            $("#btnCreateNewWord").prop('disabled', true);
            $("#btnCreateNewWord").attr("class", "btn btn-danger");
            setNewWordPanel(_allWords[index].TargetWord, _allWords[index].Meaning, _allWords[index].Id, _allWords[index].WrittenByMe);
            return;
        }

    }
    if (e.keyCode == 13) {
        takeDecisionForNewInformation(e);

        return false;
    }
    else if (e.keyCode == 27) {
        var wordId = $('#lblWordId').val();
        updateWordStatusToSpecificResult(false, wordId);
        goToNextWord()
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function goToNextWord() {
    $("#ancRightArrowCarousel").click();
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function takeDecisionForNewInformation(e) {


    var item = getWordLocally(_words[_wordIndex].Id);

    _wordReviewEndTime = new Date();
    var curretWordPreviewText = _words[_wordIndex].TargetWord.toLowerCase().trim();
    curretWordPreviewFoundWord = getWordByTargetWord(curretWordPreviewText);
    curretWordPreviewFoundWord.TargetWord = curretWordPreviewFoundWord.TargetWord.toLowerCase().trim();
    curretWordPreviewFoundWord.Meaning = curretWordPreviewFoundWord.Meaning.toLowerCase().trim();

    var wordText = $('#txtWord').val();
    var wordId = $('#lblWordId').val();

    var foundWord = null;
    if (wordId == "0") {
        foundWord = getWordByTargetWord(wordText);
    } else {
        foundWord = getWord(wordId);
    }
    if (foundWord != null && foundWord != '' && foundWord.Meaning != undefined)
        saveWordLocally(foundWord);
    var meaning = $('#txtMeaning').val().toLowerCase().trim();
    if ((foundWord != null && foundWord != '' && foundWord.Meaning != undefined && wordId != "0" && this.id != 'btnCreateNewWord' )) {

        if (curretWordPreviewFoundWord.TargetWord == curretWordPreviewText && curretWordPreviewFoundWord.Meaning == meaning && this.id != 'btnUpdateWord') {
            updateWordStatusToSpecificResult(true, _words[_wordIndex].Id);
            goToNextWord();
        }
        else {
            updateWord(e);
        }
    }
    else {
        createWord(e);

    }

    resetNewWordPanel();
    showWord(_wordIndex);

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function resetNewWordPanel() {
    setNewWordPanel('', '', 0, false);
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function initializeTooltips() {
    $('[data-toggle="tooltip"]').tooltip();
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function loadOrdNetData(e) {
    e.preventDefault();
    _isLoaded = true;

    var token = sessionStorage.getItem(_tokenKey);
    var headers = {};
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }

    $.ajax({
        url: '/api/Word/GetWordsInfoFromOrdNet',
        type: 'GET',
        async: true,
        headers: headers,
        success: function (html) {
            resource = html;
            alert(html);
        },
        error: function (html) {
            alert('Load resource ' + html);
        }
    });
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function closeAddWordPanel() {

    $('#successAlert').slideUp();

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function updateWordStatus(e) {
    e.preventDefault();
    var result = false;
    var wordId = _words[_wordIndex].Id;
    if (this.id === 'btnKnow') {
        result = true;
    }

    updateWordStatusToSpecificResult(result, wordId);

    goToNextWord();
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function updateWordStatusToSpecificResult(result, wordId) {

    var reviewTime = _wordReviewEndTime - _wordReviewStartTime;

    var token = sessionStorage.getItem(_tokenKey);
    var headers = {};
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }
    $.ajax({
        url: _webUrl + 'api/Word/UpdateWordStatus?knowsWord=' + result + '&wordId=' + wordId + '&reviewTime=' + reviewTime,
        type: 'get',
        async: false,
        encoding: "UTF-8",
        headers: headers,
        success: function (message) {
            _wordReviewStartTime = new Date();
        },
        error: function (err) {
            alert('updateWordStatusToSpecificResult' + err.responseText);
        }

    });

}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function loadOrderedWord() {

    if (this.id === 'ancLeftArrowCarousel') {
        _wordIndex--;
    }
    else if (this.id === 'ancRightArrowCarousel') {
        _wordIndex++;
    }

    if (_wordIndex < 0)
        _wordIndex = _wordIndex + _words.length;

    if (_wordIndex >= _words.length) {
        loadWords();
        _wordIndex = 0;
    }


    showWord(_wordIndex);
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function emptyArray(array) {

    while (array.length > 0)
        array.pop();
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function pushPartsIntoCurrentWordPartsQueue(text) {

    var splitedWords = text.split(" ");
    for (w in splitedWords) {
        var wordText = splitedWords[w];
        _currentWordsParts.push(wordText);
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function getWordInSpan(wordId, text) {

    var splitedWords = text.split(" ");
    var newWord = '';
    for (w in splitedWords) {
        var wordText = splitedWords[w];
        if (wordText.trim() == '')
            continue;
        var foundWord = getWordByTargetWord(wordText);

        var meaning = "No meaning found!";
        var part = "";
        if (foundWord != null && foundWord != '' && foundWord.Meaning != undefined) {
            meaning = foundWord.Meaning;
            part = '<span id="spn';
            part += foundWord.Id.toString()
            part += '" data-original-title="'
            part += meaning.trim()
            part += '"  data-toggle="tooltip" word-id="'
            part += foundWord.Id.toString()
            part += '"  written-by-me="'
            if (foundWord.WrittenByMe != null)
                part += foundWord.WrittenByMe.toString();
            else
                part += 'false';
            part += '" title="' + meaning.trim() + '" '
            part += '>'
            part += wordText
            part += '</span> ';
        }
        else {
            part = '<span id="spn' + wordText + '" data-original-title="' + meaning
           + '"  data-toggle="tooltip"  title="' + meaning + '" class="wordAmbiguous" written-by-me="false">' + wordText + '</span> ';
        }


        newWord += part;
    }
    return newWord;
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function showWord(i) {
    showChart();
    _numberOfAutomaticRepetitionOfCurrentWord = 0;
    _wordReviewStartTime = new Date();
    _currentWordsParts.splice(0, _currentWordsParts.length)
    var currentWord = _words[i];
    var str = currentWord.TargetWord;
    var newWord = getWordInSpan(currentWord.Id, str);

    var isAmbiguous = currentWord.IsAmbiguous != null && currentWord.IsAmbiguous;
    var className = 'word' + (isAmbiguous ? "Ambiguous" : "Normal");

    if (currentWord.StartTime != null && currentWord.StartTime != undefined)
        playVideo(currentWord);
    $('#divCarousel').empty();

    var wordId = ' <h4 >' + currentWord.Id.toString() + '</h4>';
    _newCarouselhtml = '<div class="item active" id="slide1">' +

                            '<div id="divNewWord"  class="currentWord carousel-caption ' + className + '">' +

                                '<div class="wordId" data-original-title="Id of The word" data-toggle="tooltip" >' +
                                    wordId +
                                '</div>' +
                                '<div >' +
                                        ' <p>' + newWord + '</p>'
                             + '</div>' +
                            '<div >' +
                                '<p>' +
                                    '<small>' + currentWord.Meaning + '</small>'
                             + '</p>';

    setNewWordPanel(currentWord.TargetWord, currentWord.Meaning, currentWord.Id, currentWord.WrittenByMe);

    showWordImages(i);
    var token = sessionStorage.getItem(_tokenKey);
    var headers = {};
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }


    $.ajax({
        url: _webUrl + 'api/Word/LoadRelatedSentences?wordId=' + currentWord.Id,
        type: 'post',
        async: false,
        encoding: "UTF-8",
        headers: headers,
        success: function (sentences) {

            var index = 0;
            _newCarouselhtml = _newCarouselhtml + '<p>';
            for (index = 0; index < sentences.length; index++) {
                var spannedSentence = getWordInSpan(sentences[index].Id, sentences[index].TargetWord);
                _newCarouselhtml = _newCarouselhtml + spannedSentence + " = <span> <small>"
                    + sentences[index].Meaning + " _  </small></span>";
            }
            _newCarouselhtml = _newCarouselhtml + '</p>'
        },
        error: function (err) {
            alert('showWord' + err.responseText);
        }

    });

    _newCarouselhtml = _newCarouselhtml + '</div>';
    $('#divCarousel').append(_newCarouselhtml);

    $("span").hover(wordSpanHover, null);
    initializeTooltips();

    emptyArray(_currentWordsParts);
    pushPartsIntoCurrentWordPartsQueue(currentWord.TargetWord);
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function setNewWordPanel(targetWord, meaning, id, writtenByMe) {
    playText(targetWord);
    $("#txtWord").val($.trim(targetWord));
    $("#txtMeaning").val($.trim(meaning));
    if (id == null || id == 'unefined')
        id = 0;
    $("#lblWordId").val($.trim(id));
    if (writtenByMe == null || writtenByMe == 'undefined' || writtenByMe == "false")
        writtenByMe = false;
    $("#chkWrittenByMe").prop('checked', writtenByMe);

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function showImagesForWord(currentWord) {

    var targetDirectory = getWordDirectory(currentWord.TargetWord.trim());

    Math.random()
    $('#images-frame').empty();
    for (var i = 1; i <= _numberOfImages; i++) {

        var theSrc = targetDirectory + i + ".jpg";
        var imageId = '#img' + i.toString();

        if (fileExists(theSrc)) {


            $(imageId).attr("src", theSrc);
            $(imageId).attr("alt", currentWord.TargetWord.trim());
            $(imageId).attr("data-toggle", "tooltip");
            $(imageId).attr("data-original-title", currentWord.TargetWord.trim());

        }
        else {

            theSrc = '/images/noImage.jpg';
            $(imageId).attr("src", theSrc);
            $(imageId).attr("alt", "No image available");
            $(imageId).attr("data-toggle", "tooltip");
            $(imageId).attr("data-original-title", "No Meanning found");

        }


        $('#images-frame').append('<div class="imageBox">' +
                '<img src=' + theSrc + ' class="wordImage" />' +
              '</div>')
    }
    initializeTooltips();
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function showWordImages(i) {

    var currentWord = _words[i];

    showImagesForWord(currentWord)
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function animateImage() {
    var img = $(this);

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function fileExists(image_url) {

    var http = new XMLHttpRequest();

    http.open('HEAD', image_url, false);
    http.send();

    return http.status != 404;

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function getWordDirectory(word) {
    var i = 0;
    var result = "/words/"
    for (i = 0; i < word.length ; i = i + 1) {
        result = result + word.substring(i, i + 1) + "/";
    }
    return result;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function wordSpanHover() {
    var wordText = $(this).text().trim();
    var wordId = $(this).attr("word-id");
    var meaning = $(this).attr("data-original-title");

    if (wordId != undefined || meaning != undefined) {
        setNewWordPanel(wordText, meaning, wordId, $(this).attr("written-by-me"));

        var currentWord = { TargetWord: wordText, Id: wordId };
        showImagesForWord(currentWord);
        playText(wordText);

    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function getWord(wordId) {

    var result = '';
    var token = sessionStorage.getItem(_tokenKey);
    var headers = {};
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }

    var wordText = '';
    $.ajax({
        url: _webUrl + 'api/Word/GetWord?id=' + wordId + '&targetWord=' + wordText,
        type: 'get',
        async: false,
        headers: headers,
        dataType: "json",
        scriptCharset: "utf-8",
        contentType: "application/x-www-form-urlencoded;charset=utf-8",
        encoding: "UTF-8",
        success: function (foundWord) {
            if (foundWord != null)
                result = foundWord
        },
        error: function (err) {
            alert('getWord:' + err);
        }

    });

    return result;

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function getReviewHistory() {

    var result = '';
    var token = sessionStorage.getItem(_tokenKey);
    var headers = {};
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }

    $.ajax({
        url: _webUrl + 'api/Word/GetReviewHistory',
        type: 'post',
        async: false,
        headers: headers,
        dataType: "json",
        scriptCharset: "utf-8",
        contentType: "application/json; charset=utf-8",
        encoding: "UTF-8",
        success: function (histories) {

            result = histories;
            $.each(result, function (i, j) {
                _xAxisData.push(j.Date);
                _yAxisData.push(j.Count);
            });
            console.log(_xAxisData);
            console.log(_yAxisData);
            return result;
        },
        error: function (err) {
            alert('GetReviewHistory:' + err);
        }

    });
    return result;


}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function getWordByTargetWord(word) {

    var result = '';
    var token = sessionStorage.getItem(_tokenKey);
    var headers = {};
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }

    $.ajax({
        url: _webUrl + 'api/Word/GetWordByTargetWord',
        type: 'post',
        async: false,
        headers: headers,
        dataType: "json",
        data: { "Id": 0, "TargetWord": word, "Meaning": '' },
        scriptCharset: "utf-8",
        contentType: "application/x-www-form-urlencoded;charset=utf-8",
        encoding: "UTF-8",
        success: function (foundWord) {
            if (foundWord != null)
                result = foundWord
        },
        error: function (err) {
            alert('getWordByTargetWord:' + err);
        }

    });

    return result;

}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function playTextByLang(wordText, lang) {

    var i = 0;
    var url = '/Words/';
    for (i = 0; i < wordText.length ; i++) {
        url += wordText[i] + '/';
    }

    url += lang + wordText + '.mp3';

    var audio = new Audio(url);
    audio.play();

}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function playText(wordText) {

    var i = 0;
    var url = '/Words/';
    for (i = 0; i < wordText.length ; i++) {
        url += wordText[i] + '/';
    }

    url += wordText + '.mp3';

    if (fileExists(url)) {
        var audio = new Audio(url);
        audio.play();
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function strTimeToSecond(strTime) {
    var parts = strTime.split(':'); // split it at the colons

    // minutes are worth 60 seconds. Hours are worth 60 minutes.
    var secondPart = parts[2].split('.')[0];
    var seconds = (+parts[0]) * 60 * 60 + (+parts[1]) * 60 + (+secondPart);

    return seconds;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function playVideo(word) {

    $("#vidMain").attr("style", "");
    var seconds = strTimeToSecond(word.StartTime);
    var intSecond = parseInt(seconds);
    var videoNumber = Math.floor(intSecond / 10) + 1;

    var splitedParts = word.MovieName.split("/");
    var i = 0;
    for (i = splitedParts.length - 1; i >= 0 ; i--) {
        if (splitedParts[i].trim().length > 0)
            break;
    }

    var targetFile = "/video/" + word.MovieName + "/" + splitedParts[i] + "_" + videoNumber.toString() + "_C.ogv";
    targetFile = targetFile.replace("//", "/")
    //if (fileExists(targetFile)) {
    $("#vidMain").attr("src", targetFile);
    $("#vidMainSrc").attr("src", targetFile);

    var vid = document.getElementById("vidMain");
    var currentVideoTime = (intSecond - 10 * videoNumber + 10) - 1;
    if (currentVideoTime < 0)
        currentVideoTime = 0;
    vid.currentTime = currentVideoTime;
    vid.play();

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function showAddWordDiv(e) {

    e.preventDefault();

    $('#successAlert').slideDown();



}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function loadAllWords(containText) {

    if (containText.length < 1)
        return;

    var token = sessionStorage.getItem(_tokenKey);
    var headers = {};
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }
    $.ajax({
        url: _webUrl + 'api/Word/GetAllWords?containText=' + containText,
        type: 'get',
        async: true,
        scriptCharset: "utf-8",
        headers: headers,
        contentType: "application/json",
        encoding: "UTF-8",
        success: function (words) {
            _allWords = words;

            setAutoComplete();

        },
        error: function (err) {
            alert('loadAllWords:' + err);
        }

    });
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function setAutoComplete() {
    var $searchBox = $('#txtWord');
    var containText = $('#txtWord').val();

    $searchBox.autocomplete({
        select: function (event, ui) {
            _allWords.map(function (word) {

                if (word.TargetWord.toUpperCase() === ui.item.value.toUpperCase()) {
                    {
                        setNewWordPanel(word.TargetWord, word.Meaning, word.Id, word.WrittenByMe);

                        return word.Meaning;
                    }
                }
            })


        },
        source: function (request, response) {
            _targetWordArr = _allWords.map(function (word) { return word.TargetWord; })

            var matches = $.map(_targetWordArr, function (tag) {
                if (tag.toUpperCase().indexOf(request.term.toUpperCase()) >= 0) {
                    {
                        return tag;
                    }
                }
            });
            response(matches);
        }
    });
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function importFile(e) {

    e.preventDefault();

    $('#importFile').slideDown();

}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function loadWords() {

    var token = sessionStorage.getItem(_tokenKey);
    var headers = {};
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }

    $.ajax({
        url: _webUrl + 'api/Word/GetWordsForReview',
        type: 'get',
        async: false,
        scriptCharset: "utf-8",
        contentType: "application/json",
        headers: headers,
        encoding: "UTF-8",
        success: function (words) {
            _words = words;
            _wordIndex = 0;
            showWord(_wordIndex);


        },
        error: function (err) {
            alert('loadWords:' + err.responseText);
        }

    });

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function createGraph(e) {

    e.preventDefault();

    $.ajax({
        url: _webUrl + 'api/Word/CreateGraph',
        type: 'get',
        async: false,
        dataType: "json",
        headers: headers,
        scriptCharset: "utf-8",
        contentType: "application/x-www-form-urlencoded;charset=utf-8",
        encoding: "UTF-8",
        success: function (html) {

        }
    });

}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function setAmbiguous(e) {

    e.preventDefault();

    var wordId = _words[_wordIndex].Id;


    $.ajax({
        url: _webUrl + 'api/Word/SetWordAmbiguous?wordId=' + wordId,
        type: 'get',
        async: false,
        scriptCharset: "utf-8",
        headers: headers,
        contentType: "application/x-www-form-urlencoded;charset=utf-8",
        encoding: "UTF-8",
        success: function (result) {
            _words[_wordIndex].IsAmbiguous = true;

            showResult(result);
            _wordIndex = _wordIndex + 1;
            showWord(_wordIndex);

        },
        error: function (html) {
            showResult(false);

        }
    });

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function deleteWord(e) {

    e.preventDefault();

    var wordId = $('#lblWordId').val()

    var token = sessionStorage.getItem(_tokenKey);
    var headers = {};
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }
    $.ajax({
        url: _webUrl + 'api/Word/DeleteWord?id=' + wordId,
        type: 'get',
        async: false,
        scriptCharset: "utf-8",
        contentType: "application/x-www-form-urlencoded;charset=utf-8",
        encoding: "UTF-8",
        headers: headers,
        success: function (result) {

            showResult(result);
            _wordIndex = _wordIndex + 1;
            showWord(_wordIndex);

        },
        error: function (html) {
            showResult(false);

        }
    });

}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function getWordLocally(id) {


    var oldWord = { "Id": '', "TargetWord": '', "Meaning": '', "WrittenByMe": '' }

    oldWord.Id = id;
    var idStr = id.toString();
    oldWord.Meaning = sessionStorage.getItem(idStr + "Meaning");
    oldWord.TargetWord = sessionStorage.getItem(idStr + "Word");

    var writtenByMe = sessionStorage.getItem(idStr + "WrittenByMe");
    if (writtenByMe == "null")
        oldWord.WrittenByMe = null;
    else
        oldWord.WrittenByMe = Boolean(writtenByMe);
    return oldWord;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function saveWordLocally(word) {

    var id = word.Id.toString();
    sessionStorage.setItem(id + "Meaning", word.Meaning);
    sessionStorage.setItem(id + "Word", word.TargetWord);
    sessionStorage.setItem(id + "WrittenByMe", word.WrittenByMe);

}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function updateWord(e) {

    e.preventDefault();


    var word = $('#txtWord').val();
    var meaning = $('#txtMeaning').val();
    var wordId = $('#lblWordId').val();
    var writtenByMe = $('#chkWrittenByMe').is(":checked");

    var token = sessionStorage.getItem(_tokenKey);
    var headers = {};
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }

    $.ajax({
        url: _webUrl + 'api/Word/UpdateWord',
        type: 'post',
        async: false,
        dataType: "json",
        data: {
            "Id": wordId, "TargetWord": word, "Meaning": meaning, "WrittenByMe": writtenByMe
        },
        scriptCharset: "utf-8",
        contentType: "application/x-www-form-urlencoded;charset=utf-8",
        encoding: "UTF-8",
        headers: headers,
        success: function (result) {

            addRecentWord(word, meaning)
            updateWordStatusToSpecificResult(true, wordId);
            showResult(result);
            if (wordId == _words[_wordIndex].Id) {
                _words[_wordIndex].TargetWord = word;
                _words[_wordIndex].Meaning = meaning;
                _words[_wordIndex].WrittenByMe = writtenByMe;
            }
            
        },
        error: function (html) {
            showResult(false);

        }
    });

    showWord(_wordIndex);

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function createWord(e) {

    e.preventDefault();

    var word = $('#txtWord').val();
    var meaning = $('#txtMeaning').val();
    var writtenByMe = $('#chkWrittenByMe').is(":checked");

    addWord(showResult, word, meaning, writtenByMe);
    //showWord(_wordIndex);
    //showAddWordDiv(e);

}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function removeAmbiguousSpan(text) {
    $('span').map(function (index, domElement) {
        if ($(domElement).text() == text) {
            $(domElement).removeClass("wordAmbiguous");
        }
    });

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function showResult(result) {

    var date = new Date().toString("yyyy-MM-dd HH:mm:ss");
    if (result == true) {

        $('#txtWord').val('');
        $('#txtMeaning').val('');
        $("#chkWrittenByMe").prop('checked', false);

        $('#divResult').removeClass("alert-danger");
        $('#divResult').addClass("alert-success");
        $('#divResult').text(resource.addWordSuccessMsg + " " + date.toString());

    } else {

        showError(' Error adding new word ');
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function showError(text) {

    var date = new Date();

    $('#divResult').removeClass("alert-success");
    $('#divResult').addClass("alert-danger");
    $('#divResult').text(text + ' ' + date.toTimeString().split(' ')[0]);
    showAddWordDiv(null);
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//function loadResource() {
//    var url = "http://ordnet.dk/ddo/ordbog?query=nedb�r";



//    $("#testAlert").slideDown();

//    var token = sessionStorage.getItem(_tokenKey);
//    var headers = {};
//    if (token) {
//        headers.Authorization = 'Bearer ' + token;
//    }

//    $.ajax({
//        url: _webUrl + 'resource.json',
//        type: 'get',
//        async: false,
//        dataType: "json",
//        scriptCharset: "utf-8",
//        contentType: "application/x-www-form-urlencoded;charset=utf-8",
//        encoding: "UTF-8",
//        headers:headers,
//        success: function (html) {
//            resource = html;
//        }
//    });
//}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function addWord(callback, targetWord, meaning, writtenByMe) {

    if (wordExist(targetWord)) {
        showError("this word currently exists");
        return;
    }
    var token = sessionStorage.getItem(_tokenKey);
    var headers = {};
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }

    $.ajax({
        url: _webUrl + 'api/Word/PostWord',
        type: 'post',
        async: false,
        dataType: "json",
        data: { "Id": "1", "TargetWord": targetWord, "Meaning": meaning, "WrittenByMe": writtenByMe },
        scriptCharset: "utf-8",
        contentType: "application/x-www-form-urlencoded;charset=utf-8",
        encoding: "UTF-8",
        headers: headers,
        success: function (result) {
            _wordReviewStartTime = new Date();
            addRecentWord(targetWord, meaning)
            callback(result);



        },
        error: function (html) {
            callback(false);

        }
    });

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function addRecentWord(targetWord, meaning) {

    var time = new Date();
    var strTime = time.getHours() + ":" + time.getMinutes() + ":" + time.getSeconds()
    _recentWords.push(targetWord + '    <small>(' + strTime + ')</small>  ');
    _recentWordsMeanings.push(meaning);

    var ind = 0;
    $('#divRecentlyAddedWords').empty();
    for (ind = _recentWords.length - 1; ind >= 0 ; ind--) {
        var _newItem = '<a href="#" class="list-group-item">';
        _newItem = _newItem + '<h4 class="list-group-item-heading">' + _recentWords[ind] + '</h4>';
        _newItem = _newItem + '<p class="list-group-item-text">' + _recentWordsMeanings[ind] + '</p>';
        _newItem = _newItem + '</a>';
        $('#divRecentlyAddedWords').append(_newItem);
    }

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//function batchImport(e) {

//    e.preventDefault();

//    var word = $('#txtWord').val();
//    var meaning = $('#txtMeaning').val();


//    var date = new Date().toString("yyyy-MM-dd HH:mm:ss");
//    $.ajax({
//        url: _webUrl + 'api/Word/PostWord',
//        type: 'post',
//        async: false,
//        dataType: "json",
//        data: { "Id": "1", "TargetWord": word, "Meaning": meaning },
//        scriptCharset: "utf-8",
//        contentType: "application/x-www-form-urlencoded;charset=utf-8",
//        encoding: "UTF-8",
//        success: function (result) {
//            showResult(html);
//        },
//        error: function (html) {
//            showResult(false);
//        }
//    });
//}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function showChart() {

    var result = getReviewHistory();
    var arr = new Array();
    $('#visualisation').empty();

    var totalHistories = 0;
    $.each(result, function (i, j) {
        var x = parseInt(j.X);
        if (x < 30) {
            arr.push({ "x": x, "y": parseInt(j.Y) });
        }
        totalHistories += j.Y;
    });
    lineData = arr;//JSON.stringify(arr);
    $("#lblTodaywords_TotalWords").val(arr[0].y.toString() + "-" + totalHistories);
    var vis = d3.select('#visualisation'),
    WIDTH = 1000,
    HEIGHT = 200,
    MARGINS = {
        top: 20,
        right: 60,
        bottom: 20,
        left: 250
    },
    xRange = d3.scale.linear().range([MARGINS.left, WIDTH - MARGINS.right]).domain([d3.min(lineData, function (d) {
        return d.x;
    }), d3.max(lineData, function (d) {
        return d.x;
    })]),
    yRange = d3.scale.linear().range([HEIGHT - MARGINS.top, MARGINS.bottom]).domain([d3.min(lineData, function (d) {
        return d.y;
    }), d3.max(lineData, function (d) {
        return d.y;
    })]),
    xAxis = d3.svg.axis()
      .scale(xRange)
      .tickSize(5)
      .tickSubdivide(true),
    yAxis = d3.svg.axis()
      .scale(yRange)
      .tickSize(5)
      .orient('left')
      .tickSubdivide(true);

    vis.append('svg:g')
      .attr('class', 'x axis')
      .attr('transform', 'translate(0,' + (HEIGHT - MARGINS.bottom) + ')')
      .call(xAxis);

    vis.append('svg:g')
      .attr('class', 'y axis')
      .attr('transform', 'translate(' + (MARGINS.left) + ',0)')
      .call(yAxis);

    var lineFunc = d3.svg.line()
  .x(function (d) {
      return xRange(d.x);
  })
  .y(function (d) {
      return yRange(d.y);
  })
  .interpolate('linear');

    vis.append('svg:path')
  .attr('d', lineFunc(lineData))
  .attr('stroke', 'green')
  .attr('stroke-width', 2)
  .attr('fill', 'none');
    //getReviewHistory();
}
