'use strict';
app.factory('utils', function () {
  function unique(arr) {
    var result = [],
      isRepeated;
    for (var i = 0, len = arr.length; i < len; i++) {
      isRepeated = false;
      for (var j = 0, len = result.length; j < len; j++) {
        if (arr[i] == result[j]) {
          isRepeated = true;
          break;
        }
      }
      if (!isRepeated) {
        result.push(arr[i]);
      }
    }
    return result;
  };
  //判断object是否为空对象，返回空对象返回true
  function isobjval(object) {
    return JSON.stringify(object) == "{}";
  };
  //返回obg里key的个数
  function keynum(object) {
    return Object.keys(object).length;
  };
  return {
    uniqueArr: unique, //去掉数组中的重复项
    isobjval: isobjval, //判断对象是否为空
    keynum: keynum
  };
}).factory('CONSTANTS', function () {
  var uibase = '/App_Plugins/SEACompliance/backoffice/SEACompliance/';
  var apibase = '/pssweb/';
  return {
    uibase: uibase,
    apibase: apibase
  };
}).factory('CONFIRM', function ($modal, CONSTANTS) {
  // Use:
  // CONFIRM.open('hi','hello world').result.then(function (res) {
  //   debugger
  // }, function () {
  //   $log.info('Modal dismissed at: ' + new Date());
  // });
  function open(title, content) {
    var modalInstance = $modal.open({
      animation: true,
      templateUrl: CONSTANTS.uibase + '/common/confirm.html',
      controller: function ($scope, $modalInstance, items) {
        $scope.itemObj = items;
        $scope.ok = function () {
          $modalInstance.close('ok');
        };
        $scope.cancel = function (index) {
          $modalInstance.dismiss('cancel');
        };
      },
      resolve: {
        items: function () {
          return {
            title: title,
            content: content
          };
        }
      }
    });
    return modalInstance;
  };
  return {
    open: open
  };
}).config(['$validationProvider', function ($validationProvider) {
  $validationProvider.showSuccessMessage = false; // or true(default)
  $validationProvider.showErrorMessage = true; // or true(default)
  $validationProvider.addMsgElement = function (element) {
    // Insert my own Msg Element
    $(element).parent().append('<span></span>');
  };
  $validationProvider.getMsgElement = function (element) {
    return $(element).parent().children().last();
  };
}]).directive('sinderHtml', function ($compile) {
  // 代替ng-bind-html进行html的渲染，有BUG，不能监听自动渲染
  return {
    restrict: 'A',
    replace: true,
    link: function (scope, ele, attrs) {
      scope.$watch('attrs.sinderHtml', function(html) {
        debugger
        ele.html(attrs.sinderHtml);
        $compile(ele.contents())(scope);
      });
    }
  };
});