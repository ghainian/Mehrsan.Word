
//var ImagesFrame = React.createClass({
//    showImage: function () {
        
        
//    },
//    render: function() {
//        var images = [];
//        var targetDirectory = this.props.getWordDirectory();
//        for (var i = 1; i <= 20; i++) {
//            //var theSrc = "/Words/a/l/t/s/å/" + i + ".jpg";
//            var theSrc = targetDirectory + i + ".jpg";

//            images.push(
//              <div className="imageBox">
//                <img src={theSrc} className="wordImage" />
//              </div>
//          );
//        }

//        return (
//          <div >
//            <div >
//              {images}
//            </div>
//          </div>
//      );
//    }

//});


//var Game = React.createClass({

//    getInitialState: function() {
//        return { }
//    },
//    getWordDirectory: function () {
//        var word = $("#txtWord").val();
//        var i = 0;
//        var result = "words/"
//        for (i = 0; i < word.length ; i = i + 1) {
//            result = result + word.substring(i, i+1) + "/";
//        }
//        return result;
//    },
//    render: function () {
//    return (
//          <ImagesFrame 
//                       getWordDirectory={this.getWordDirectory}/>
//    );
//  }
//});

//React.render(
//  <Game />,
//  document.getElementById('divWordImages')
//);
