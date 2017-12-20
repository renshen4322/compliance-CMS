app.controller("editCheckItemCtrl", function(
  $scope,
  $modalInstance,
  items,
  FileUploader,
  recordResource,
  notificationsService
) {
  function RemoveValidateDiv(_scrollTo) {
    $("#" + _scrollTo + " iframe").css({
      border: "0px"
    });
  }
  // 轮询开启监听
  var watchFunc = function(watchVaule, className) {
    var watchContent = $scope.$watch(watchVaule, function(newValue, oldValue) {
      if (newValue != undefined && oldValue != undefined) {
        if (newValue && moreSpace(newValue, className, "loop")) {
          RemoveValidateDiv(className);
          watchContent();
        }
      }
    });
  };
  //滚动到指定div位置
  function ValidateDiv(_scrollTo) {
    if (!_scrollTo) {
      return;
    }
    // $("#" + _scrollTo + '').addClass("invalidFieldError iframe");
    $("#" + _scrollTo + " iframe").css({
      border: "1px solid red"
    });
    var container = $(".modal-body");
    var scrollToDiv = $("#" + _scrollTo);
    container.scrollTop(
      scrollToDiv.offset().top - container.offset().top + container.scrollTop()
    );
  }
  // 如果有多个空格，返回true
  var moreSpace = function(value, className, type) {
    var firstEle = value.substring(0, 3);
    var lastEle = value.substring(value.length - 4);
    var middleEle = value.substring(3, value.length - 4);
    if (firstEle === "<p>" && lastEle === "</p>") {
      if (middleEle.trim() === "") {
        ValidateDiv(className);
        var watchStr = "editObj." + className + "[0].value";
        var sdsdd = $scope.editObj[className][0].value;
        // 防止多次开启重复的监听
        if (!type) {
          watchFunc(watchStr, className);
        }
        return false;
      }
    }
    // 正常返回
    return true;
  };
  // 通用的验证方法
  var CheckItemFuc = function(className) {
    // 没有输入任何文本时
    if (!$scope.editObj[className][0].value) {
      ValidateDiv(className);
      var watchStr = "editObj." + className + "[0].value";
      var sdsdd = $scope.editObj[className][0].value;
      watchFunc(watchStr, className);
      return false;
    } else {
      // 输入多个空格时
      return moreSpace($scope.editObj[className][0].value, className);
    }
  };
  $scope.ok = function(isNewCheckItemVersion) {
    if (!CheckItemFuc("checkItemContent")) {
      return;
    }
    if (!CheckItemFuc("penalty")) {
      return;
    }
    if (!CheckItemFuc("reasonCodes")) {
      return;
    }
    if (isNewCheckItemVersion === "newVersion") {
      $scope.editObj.isNewCheckItemVersion = true;
    }
    $modalInstance.close($scope.editObj);
  };
  $scope.cancel = function() {
    $modalInstance.dismiss("cancel");
  };
  var init = function() {
    var propertyConfig = {
      editor: {
        toolbar: [
          // "code",
          "undo",
          "redo",
          "cut",
          "bold",
          "italic",
          "underline",
          // "alignleft",
          // "aligncenter",
          // "alignright",
          // "bullist",
          // "numlist",
          // "table",
          "link",
          "fontselect",
          "fontsizeselect",
          "forecolor"
        ],
        stylesheets: [],
        dimensions: { height: 140, width: "90%" }
      }
    };
    var initEiit = function(val) {
      if (!val) {
        val = "";
      }
      return [
        {
          view: "rte",
          config: propertyConfig,
          hideLabel: true,
          value: val
        }
      ];
    };
    var editInput = [
      {
        view: "rte",
        config: propertyConfig,
        hideLabel: true,
        value: ""
      }
    ];
    if (items.type != "add") {
      // edit
      var tempEditObj = angular.copy(items);
      // 新建的document的新checkItem不能保存新的checkItem版本
      tempEditObj.checkItemContent = angular.copy(
        initEiit(tempEditObj.CheckItemContent)
      );
      tempEditObj.penalty = angular.copy(initEiit(tempEditObj.Penalty));
      tempEditObj.reasonCodes = angular.copy(initEiit(tempEditObj.ReasonCodes));
      $scope.editObj = tempEditObj;
    } else {
      // new doc
      $scope.editObj = {
        type: "add",
        Importance: 1,
        UnderReview: 0,
        checkItemContent: angular.copy(initEiit()),
        penalty: angular.copy(initEiit()),
        reasonCodes: angular.copy(initEiit())
      };
    };
    // 新创建的document不能让新添加的checkItem创建新版本
    if ($scope.editObj.type === 'add' || items.routeParamsId < 0) {
      $scope.isShowNewVersion = false;
    } else {
      $scope.isShowNewVersion = true
    }
    setTimeout(function() {
      $(".modal").css({
        width: "1000px",
        left: "34%"
      });
    }, 60);
  };
  init();
});
