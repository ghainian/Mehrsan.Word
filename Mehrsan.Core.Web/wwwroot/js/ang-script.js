
var myApp = angular.module("myModule", ['ngSanitize'])

    .controller('myController', ['$scope', '$interval', '$http', '$sce',
        function ($scope, $interval, $http, $sce) {


            //$("#UserId").autocomplete({
            //    source: '@Url.Action("GetUserClaims")',
            //    minLength = 2
            //});

            $scope.chkPronounceWholeWordChecked = true;
            var _addWordError = ' Error adding new word '
            var stop;
            var _wordIndex = 0;
            var _words = [];
            var _newCarouselhtml = '';
            var _webUrl = 'http://localhost:44378/';//window.location.href;
            var _numberOfImages = 20;
            var _allWords = '';
            var _targetWordArr = '';
            var _recentWords = new Array();
            var _currentWordsParts = new Array();
            //var _recentWordsMeanings = new Array();
            var _wordIndexToPlay = 0;
            var _wordRepeationThresholdForLearning = 3;
            var _numberOfAutomaticRepetitionOfCurrentWord = 0;
            var _playWordQueueInterval = 3000;
            var _tokenKey = 'accessToken';
            var _lock = 1;
            var _intervalHandle = 0;
            var _xAxisData = new Array();
            var _yAxisData = new Array();
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function callController() {

                if (_words == null || _words.length == 0) {
                    loadWords();
                }

            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function initializeTooltips() {
                $('[data-toggle="tooltip"]').tooltip();
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


            function initialize() {

                if (_lock == 1) {
                    _lock = 0;
                    var sec = _playWordQueueInterval / 1000;
                    $scope.txtWordRepeationIntervalContent = sec.toString();

                    $scope.txtNumberOfAutomaticRepetitionsContent = _numberOfAutomaticRepetitionOfCurrentWord.toString();
                    $scope.txtWordRepeationThresholdForLearningContent = _wordRepeationThresholdForLearning.toString();
                    $('#btnSignup').click(signup);
                    $('.dropdown-toggle').dropdown();
                    var resource = null;
                    $('li').on('click', setWordReviewStrategy);
                    'use strict';
                    //clearInterval();

                    callController();

                    $("#btnUndo").click(undo);

                    $('#importFile').slideUp();

                    $('#successAlert').slideDown();

                    $('#btnAddWord').click(showAddWordDiv);

                    $('#btnImportFile').click(importFile);

                    $('#btnCreateGraph').click(createGraph);
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


            initialize();

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            function loadWords() {

                var token = sessionStorage.getItem(_tokenKey);
                var headers = {};
                if (token) {
                    headers.Authorization = 'Bearer ' + token;
                }

                $http({
                    method: 'GET',
                    url: _webUrl + 'Home/GetWordsForReview',
                    async: false,
                    //scriptCharset: "utf-8",
                    contentType: "application/json",
                    headers: { 'Authorization': 'Bearer ' + token },
                    //encoding: "UTF-8",


                }).then(function successCallback(response) {
                    _words = response.data;
                    _wordIndex = 0;
                    showWord(_wordIndex);

                    $scope.playWordQueue = function () {
                        // Don't start a new fight if we are already fighting
                        if (angular.isDefined(stop)) return;

                        func = $interval(function () {
                            playWord();
                        }, parseInt(_wordRepeationThresholdForLearning) * 1000);
                    };

                }, function errorCallback(err) {
                    alert('loadWords:' + err.responseText);
                });


            }




            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function playWord() {

                var sec = parseInt($scope.txtWordRepeationIntervalContent);

                _wordRepeationThresholdForLearning = sec;
                var pronounceWholeWord = $scope.chkPronounceWholeWordChecked;
                var pronounceWordAndMeaning = $scope.chkPronounceWordAndMeaningChecked;


                var wordText = _words[_wordIndex].id.toString();
                if (pronounceWholeWord) {

                    if (_currentWordsParts.length > 1) {
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
                        updateWordStatusToSpecificResult(true, _words[_wordIndex].id);
                        goToNextWord();

                    }

                    $scope.$watch(function () {
                        $scope.txtNumberOfAutomaticRepetitionsContent = _numberOfAutomaticRepetitionOfCurrentWord.toString();

                    });
                }

                _wordIndexToPlay = (_wordIndexToPlay + 1) % _currentWordsParts.length;


            }


            //$(function () {
            //    initialize();
            //    //loadResource();

            //});
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            $scope.check = function () {
                console.log($scope.txtWordRepeationThresholdForLearningContent); //works
                console.log($scope.txtWordRepeationIntervalContent); //works
                console.log('-----------------------------------------------------------------------'); //works
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            function initialize() {

                if (_lock == 1) {
                    _lock = 0;
                    var sec = _playWordQueueInterval / 1000;
                    $scope.txtWordRepeationIntervalContent = sec.toString();

                    $scope.txtNumberOfAutomaticRepetitionsContent = _numberOfAutomaticRepetitionOfCurrentWord.toString();
                    $scope.txtWordRepeationThresholdForLearningContent = _wordRepeationThresholdForLearning.toString();
                    $('#btnSignup').click(signup);
                    $('.dropdown-toggle').dropdown();
                    var resource = null;
                    $('li').on('click', setWordReviewStrategy);
                    'use strict';
                    //clearInterval();

                    callController();

                    //$("#txtMeaning").on("input propertychange paste", meaningBoxKeyDown);
                    //$("#txtWord").on("input propertychange paste", meaningBoxKeyDown);
                    //$("#txtMeaning").on("keypress kyup keydown", meaningBoxKeyDown);
                    //$("#txtWord").on("keypress kyup keydown", meaningBoxKeyDown);

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

            function addImageBoxes() {
                return;//tis image is not supported currently
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

                var id = _words[_wordIndex].id;
                var oldWord = getWordLocally(id);

                _words[_wordIndex].targetWord = oldWord.targetWord;
                _words[_wordIndex].writtenByMe = oldWord.writtenByMe;
                _words[_wordIndex].meaning = oldWord.meaning;

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


                $http({
                    method: 'POST',
                    url: _webUrl + 'Account/ChangePassword',
                    data: changePassData,
                    headers: { 'Authorization': 'Bearer ' + token },

                }).then(function successCallback(response) {
                    alert("Change Password finished successfully");

                }, function errorCallback(err) {
                    if (e2.status != 200)
                        alert('ChangePasswordError ' + e2.responseText.toString());
                    else
                        alert("Change Password finished successfully with status Code 200");
                });
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function signup() {

                var email = $("#txtEmailSignup").val();
                var password = $("#txtPasswordSignup").val();

                $http({
                    method: 'POST',
                    url: _webUrl + 'Account/Register',
                    data: { "Email": email, "Password": password, "ConfirmPassword": password },
                    headers: { 'Authorization': 'Bearer ' + token },


                }).then(function successCallback(response) {
                    alert("Signup finished successfully");
                    loginFunction(email, password);

                }, function errorCallback(err) {
                    if (e2.status != 200)
                        alert('Signup ' + e2.toString());
                });

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
                    url: '/Identity/Account/login',
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

            function register() {


                email = "ghainian@gmail.com"
                pass = "Ghainian1@gmail.com"
                var regData = {
                    username: email,
                    password: pass
                };
                var token = sessionStorage.getItem(_tokenKey);
                $.ajax({
                    url: _webUrl + 'Identity/Account/Register',
                    type: 'post',
                    async: false,
                    dataType: "json",
                    data: regData,
                    scriptCharset: "utf-8",
                    contentType: "application/x-www-form-urlencoded;charset=utf-8",
                    encoding: "UTF-8",
                    headers: { 'Authorization': 'Bearer ' + token },
                    success: function (result) {
                        setStartTime()
                        addRecentWord(targetWord, meaning)
                        callback(result);



                    },
                    error: function (html) {
                        if (callback) {
                            callback(false);
                        }

                    }
                });
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function setStartTime() {

                if (_words[_wordIndex] && (!_words[_wordIndex].ReviewStartTime || _words[_wordIndex].ReviewStartTime == null || _words[_wordIndex].ReviewStartTime == ""))
                    _words[_wordIndex].ReviewStartTime = new Date();

            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function login() {


                var email = "ghainian3@gmail.com";//$("#txtEmailLogin").val();
                var password = "Dardo.delanirvanahafz852";//$("#txtPasswordLogin").val();

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

            function setWordReviewStrategy() {

                $('.dropdown-toggle').map(function (index, domElement) {
                    //alert($(domElement)[0].id);
                    if ($(domElement)[0].id == "ancWordReviewStrategy")
                        $(domElement).html($(this).html() + '<span class="caret"></span>')
                });


            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            function wordExist(text) {

                var word = getWordByTargetWord(text)
                if (word == '' || word == null)
                    return false;

                return true;

            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            function meaningBoxKeyDown(e) {

                var text = $scope.txtWordContent;
                var wordExists = wordExist(text);

                if (this.id == 'txtWord') {
                    $("#btnCreateNewWord").prop('disabled', false);
                    $("#btnCreateNewWord").attr("class", "btn btn-default")

                    loadAllWords(text);

                    if (wordExists) {
                        $("#btnCreateNewWord").prop('disabled', true);
                        $("#btnCreateNewWord").attr("class", "btn btn-danger");
                        setNewWordPanel(_allWords[index].targetWord, _allWords[index].meaning, _allWords[index].id, _allWords[index].writtenByMe);
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


                var item = getWordLocally(_words[_wordIndex].id);

                var curretWordPreviewText = _words[_wordIndex].targetWord.toLowerCase().trim();
                curretWordPreviewFoundWord = getWordByTargetWord(curretWordPreviewText);
                curretWordPreviewFoundWord.targetWord = curretWordPreviewFoundWord.targetWord.toLowerCase().trim();
                curretWordPreviewFoundWord.meaning = curretWordPreviewFoundWord.meaning.toLowerCase().trim();

                var wordText = $('#txtWord').val();
                var wordId = $('#lblWordId').val();

                var foundWord = null;
                if (wordId == "0") {
                    foundWord = getWordByTargetWord(wordText);
                } else {
                    foundWord = getWord(wordId);
                }
                if (foundWord != null && foundWord != '' && foundWord.meaning != undefined)
                    saveWordLocally(foundWord);
                var meaning = $('#txtMeaning').val().toLowerCase().trim();
                if ((foundWord != null && foundWord != '' && foundWord.meaning != undefined && wordId != "0" && this.id != 'btnCreateNewWord')) {

                    if (curretWordPreviewFoundWord.targetWord == curretWordPreviewText && curretWordPreviewFoundWord.meaning == meaning && this.id != 'btnUpdateWord') {
                        updateWordStatusToSpecificResult(true, _words[_wordIndex].id);
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


            function loadOrdNetData(e) {
                e.preventDefault();
                _isLoaded = true;

                var token = sessionStorage.getItem(_tokenKey);
                var headers = {};
                if (token) {
                    headers.Authorization = 'Bearer ' + token;
                }

                $.ajax({
                    url: '/api/Home/GetWordsInfoFromOrdNet',
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
                var wordId = _words[_wordIndex].id;
                if (this.id === 'btnKnow') {
                    result = true;
                }

                updateWordStatusToSpecificResult(result, wordId);

                goToNextWord();
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function updateWordStatusToSpecificResult(result, wordId) {

                var reviewTime = new Date() - _words[_wordIndex].ReviewStartTime;

                var token = sessionStorage.getItem(_tokenKey);
                var headers = {};
                if (token) {
                    headers.Authorization = 'Bearer ' + token;
                }

                $http({
                    method: 'GET',
                    url: _webUrl + 'Home/UpdateWordStatus?knowsWord=' + result + '&wordId=' + wordId + '&reviewTime=' + reviewTime,
                    headers: { 'Authorization': 'Bearer ' + token },

                }).then(function successCallback(response) {
                    setStartTime()

                }, function errorCallback(err) {
                    alert('updateWordStatusToSpecificResult' + err.responseText);
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
                    if (foundWord != null && foundWord != '' && foundWord.meaning != undefined) {
                        meaning = foundWord.meaning;
                        part = '<span id="spn';
                        part += foundWord.id.toString()
                        part += '" data-original-title="'
                        part += meaning.trim()
                        part += '"  data-toggle="tooltip" word-id="'
                        part += foundWord.id.toString()
                        part += '"  written-by-me="'
                        if (foundWord.writtenByMe != null)
                            part += foundWord.writtenByMe.toString();
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
                setStartTime()
                _currentWordsParts.splice(0, _currentWordsParts.length)
                $scope.currentWord = _words[i];
                var str = $scope.currentWord.targetWord;
                $scope.wordInSpan = getWordInSpan($scope.currentWord.id, str);
                $sce.trustAsHtml($scope.wordInSpan);

                $scope.isAmbiguous = $scope.currentWord.IsAmbiguous != null && $scope.currentWord.IsAmbiguous;
                
                if ($scope.currentWord.StartTime != null && $scope.currentWord.StartTime != undefined)
                    playVideo($scope.currentWord);
                //$('#divCarousel').empty();
                //wordId = currentWord.id;
                //$('#lblWordId').val(wordId.toString())
                //var wordId = ' <h4 >' + currentWord.id.toString() + '</h4>';
                //_newCarouselhtml = '<div class="item active" style="text-align:center;"  id="slide1">' +

                //    '<div >' +
                //    ' <h1>' + newWord + '</h1>'
                //    + '</div>' +
                //    '<div >' +
                //    '<p>' +
                //    '<h1>' + currentWord.meaning + '</h1>'
                //    + '</p>'
                //    + '</div>' +
                //    '<div class="wordId" data-original-title="Id of The word" data-toggle="tooltip" >' +
                //    wordId;

                setNewWordPanel($scope.currentWord.targetWord, $scope.currentWord.meaning, $scope.currentWord.id, $scope.currentWord.writtenByMe);

                //showWordImages(i);
                var token = sessionStorage.getItem(_tokenKey);
                var headers = {};
                if (token) {
                    headers.Authorization = 'Bearer ' + token;
                }

                $http({
                    method: 'POST',
                    url: _webUrl + 'Home/LoadRelatedSentences?wordId=' + $scope.currentWord.id,
                    headers: { 'Authorization': 'Bearer ' + token },


                }).then(function successCallback(response) {

                    sentences = response.data;
                    var index = 0;
                    $scope.relatedSentences = sentences
                    
                }, function errorCallback(err) {

                    alert('showWord' + err.responseText);

                });
                
                $("span").hover(wordSpanHover, null);
                initializeTooltips();

                emptyArray(_currentWordsParts);
                pushPartsIntoCurrentWordPartsQueue(currentWord.targetWord);
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function setNewWordPanel(targetWord, meaning, id, writtenByMe) {
                playText(targetWord);
                //$scope.$watch(function () {
                $scope.FormData = { txtWordContent: null }
                $scope.FormData.txtWordContent = $.trim(targetWord);
                //});
                //$scope.$apply()
                $("#txtMeaning").val($.trim(meaning));
                if (id == null || id == 'unefined')
                    id = 0;
                $("#lblWordId").val($.trim(id));
                if (writtenByMe == null || writtenByMe == 'undefined' || writtenByMe == "false")
                    writtenByMe = false;
                $scope.chkWrittenByMeChecked = writtenByMe;

            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function showImagesForWord(currentWord) {
                return;//this feature is not supported currently
                var targetDirectory = getWordDirectory(currentWord.targetWord.trim());

                Math.random()
                $('#images-frame').empty();
                for (var i = 1; i <= _numberOfImages; i++) {

                    var theSrc = targetDirectory + i + ".jpg";
                    var imageId = '#img' + i.toString();

                    if (fileExists(theSrc)) {


                        $(imageId).attr("src", theSrc);
                        $(imageId).attr("alt", currentWord.targetWord.trim());
                        $(imageId).attr("data-toggle", "tooltip");
                        $(imageId).attr("data-original-title", currentWord.targetWord.trim());

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

                try {
                    http.open('HEAD', image_url, false);
                    http.send();

                    return http.status != 404;

                } catch (e) {
                    return false;
                }
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function getWordDirectory(word) {
                var i = 0;
                var result = "/words/"
                for (i = 0; i < word.length; i = i + 1) {
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
                    url: _webUrl + 'Home/GetWord',
                    type: 'post',
                    async: false,
                    headers: headers,
                    dataType: "json",
                    data: { "id": wordId, "targetWord": wordText},
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

            function getReviewHistory(resolve, reject) {

                var result = '';
                var token = sessionStorage.getItem(_tokenKey);
                var headers = {};
                if (token) {
                    headers.Authorization = 'Bearer ' + token;
                }

                $http({
                    method: 'POST',
                    url: _webUrl + 'Home/GetReviewHistory',
                    headers: { 'Authorization': 'Bearer ' + token },


                }).then(function successCallback(response) {
                    histories = response.data
                    result = histories;
                    $.each(result, function (i, j) {
                        _xAxisData.push(j.Date);
                        _yAxisData.push(j.Count);
                    });
                    resolve(result);
                    return result;
                }, function errorCallback(err) {
                    alert('GetReviewHistory:' + err);
                    reject()
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
                    url: _webUrl + 'Home/GetWordByTargetWord',
                    type: 'post',
                    async: false,
                    headers: headers,
                    dataType: "json",
                    data: { "id": 0, "targetWord": word, "meaning": '' },
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
                for (i = 0; i < wordText.length; i++) {
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
                for (i = 0; i < wordText.length; i++) {
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
                for (i = splitedParts.length - 1; i >= 0; i--) {
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
                if (e) {
                    e.preventDefault();
                }
                $('#successAlert').slideDown();



            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function loadAllWords(containText) {

                if (containText && containText.length < 1)
                    return;

                var token = sessionStorage.getItem(_tokenKey);
                var headers = {};
                if (token) {
                    headers.Authorization = 'Bearer ' + token;
                }

                $http({
                    method: 'GET',
                    url: _webUrl + 'Home/GetAllWords?containText=' + containText,
                    headers: { 'Authorization': 'Bearer ' + token },

                }).then(function successCallback(response) {

                    words = response.data;
                    _allWords = words;

                    setAutoComplete();


                }, function errorCallback(err) {
                    alert('loadAllWords:' + err);
                });

            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function setAutoComplete() {
                var $searchBox = $('#txtWord');
                var containText = $('#txtWord').val();

                $searchBox.autocomplete({
                    select: function (event, ui) {
                        _allWords.map(function (word) {

                            if (word.targetWord.toUpperCase() === ui.item.value.toUpperCase()) {
                                {
                                    setNewWordPanel(word.targetWord, word.meaning, word.id, word.writtenByMe);

                                    return word.meaning;
                                }
                            }
                        })


                    },
                    source: function (request, response) {
                        _targetWordArr = _allWords.map(function (word) { return word.targetWord; })

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


            function createGraph(e) {

                e.preventDefault();

                $http({
                    method: 'GET',
                    url: _webUrl + 'Home/CreateGraph',
                    headers: { 'Authorization': 'Bearer ' + token },

                }).then(function successCallback(response) {


                }, function errorCallback(err) {

                });

            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function setAmbiguous(e) {

                e.preventDefault();

                var wordId = _words[_wordIndex].id;

                $http({
                    method: 'GET',
                    url: _webUrl + 'Home/SetWordAmbiguous?wordId=' + wordId,
                    headers: { 'Authorization': 'Bearer ' + token },

                }).then(function successCallback(response) {

                    result = response.data
                    _words[_wordIndex].IsAmbiguous = true;

                    showResult(result, ' Word set as ambiguous successfully');
                    _wordIndex = _wordIndex + 1;
                    showWord(_wordIndex);

                }, function errorCallback(err) {
                    showResult( ' Error setting error as ambiguous ' );
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

                    method: 'GET',
                    url: _webUrl + 'Home/DeleteWord?id=' + wordId,
                    headers: { 'Authorization': 'Bearer ' + token },
                    async: true,
                    dataType: "",
                    scriptCharset: "utf-8",
                    contentType: "application/x-www-form-urlencoded;charset=utf-8",
                    encoding: "UTF-8",
                    headers: headers,
                    success: function (result) {
                        loadWords();
                        _wordIndex = 0;
                        showWord(_wordIndex);
                        showResult(false, " Word deleted successfully ");
                    },
                    error: function (html) {
                        showResult(false, " Deleting word raised error ");

                    }
                });


            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function getWordLocally(id) {


                var oldWord = { "id": '', "targetWord": '', "meaning": '', "WrittenByMe": '' }

                oldWord.id = id;
                var idStr = id.toString();
                oldWord.meaning = sessionStorage.getItem(idStr + "meaning");
                oldWord.targetWord = sessionStorage.getItem(idStr + "Word");

                var writtenByMe = sessionStorage.getItem(idStr + "WrittenByMe");
                if (writtenByMe == "null")
                    oldWord.writtenByMe = null;
                else
                    oldWord.writtenByMe = Boolean(writtenByMe);
                return oldWord;
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function saveWordLocally(word) {

                var id = word.id.toString();
                sessionStorage.setItem(id + "meaning", word.meaning);
                sessionStorage.setItem(id + "Word", word.targetWord);
                sessionStorage.setItem(id + "WrittenByMe", word.writtenByMe);

            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function updateWord(e) {

                e.preventDefault();


                var word = $('#txtWord').val();
                var meaning = $('#txtMeaning').val();
                var wordId = $('#lblWordId').val();
                var writtenByMe = $scope.chkWrittenByMeChecked;

                var token = sessionStorage.getItem(_tokenKey);
                var headers = {};
                if (token) {
                    headers.Authorization = 'Bearer ' + token;
                }


                $.ajax({
                    url: _webUrl + 'Home/UpdateWord',
                    type: 'post',
                    async: true,
                    dataType: "json",
                    data: {
                        "id": wordId, "targetWord": word, "meaning": meaning, "writtenByMe": writtenByMe
                    },
                    scriptCharset: "utf-8",
                    contentType: "application/x-www-form-urlencoded;charset=utf-8",
                    encoding: "UTF-8",
                    headers: headers,
                    success: function (result) {

                        addRecentWord(word, meaning)
                        updateWordStatusToSpecificResult(true, wordId);
                        showResult(true, " Word status updated successfully ");
                        if (wordId == _words[_wordIndex].id) {
                            _words[_wordIndex].targetWord = word;
                            _words[_wordIndex].meaning = meaning;
                            _words[_wordIndex].writtenByMe = writtenByMe;
                        }
                        showWord(_wordIndex);
                    },
                    error: function (html) {
                        showResult(false, " Error updating word status ");
                        showWord(_wordIndex);
                    }
                });

                

            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            function createWord(e) {

                e.preventDefault();

                var word = $('#txtWord').val();
                var meaning = $('#txtMeaning').val();
                var writtenByMe = $scope.chkWrittenByMeChecked;

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

            function showResult(result,message) {

                var date = new Date().toString("yyyy-MM-dd HH:mm:ss");
                if (result == true) {

                    $('#txtWord').val('');
                    $('#txtMeaning').val('');
                    $scope.chkWrittenByMeChecked = false;

                    $('#divResult').removeClass("alert-danger");
                    $('#divResult').addClass("alert-success");
                    $('#divResult').text("Word Created/Updated successfully " + date.toString());

                } else {

                    showError(message);
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
            //    var url = "http://ordnet.dk/ddo/ordbog?query=nedbør";





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
                    url: _webUrl + 'Home/PostWord',
                    type: 'post',
                    async: true,
                    dataType: "json",
                    data: { "id": "1", "targetWord": targetWord, "meaning": meaning, "writtenByMe": writtenByMe },
                    scriptCharset: "utf-8",
                    contentType: "application/x-www-form-urlencoded;charset=utf-8",
                    encoding: "UTF-8",
                    headers: headers,
                    success: function (result) {
                        setStartTime()
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
                _recentWords.push({ TargetWord: targetWord, Meaning: meaning, Time: strTime })
                //_recentWords.push(targetWord + '    <small>(' + strTime + ')</small>  ');
                //_recentWordsMeanings.push(meaning);

                //var ind = 0;
                //$('#divRecentlyAddedWords').empty();
                //for (ind = _recentWords.length - 1; ind >= 0 ; ind--) {
                //    var _newItem = '<a href="#" class="list-group-item">';
                //    _newItem = _newItem + '<h4 class="list-group-item-heading">' + _recentWords[ind] + '</h4>';
                //    _newItem = _newItem + '<p class="list-group-item-text">' + _recentWordsMeanings[ind] + '</p>';
                //    _newItem = _newItem + '</a>';
                //    $('#divRecentlyAddedWords').append(_newItem);
                //}

            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            function showChart() {
                return;//currently we dont support this feature
                var getReviewHistoryPromise = new Promise(function (resolve, reject) {
                    getReviewHistory(resolve, reject);
                });

                getReviewHistoryPromise.then(function (result) {

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

                }, function (err) {
                    alert("an error occured in Show Chart")
                });

            }


            //myApp.controller("myController", myController);

        }]);

