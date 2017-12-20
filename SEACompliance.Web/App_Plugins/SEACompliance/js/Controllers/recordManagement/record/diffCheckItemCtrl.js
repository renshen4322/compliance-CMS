app.controller("diffCheckItemCtrl", function (
  $scope,
  $modalInstance,
  items,
  FileUploader,
  recordResource,
  notificationsService
) {
  var restSize = function () {
    setTimeout(function () {
      $(document).ready(function () {
        var height = Math.max($(".diffLeft").height(), $(".diffRight").height());
        $(".diffLeft").height(height);
        $(".diffRight").height(height);
      });
    }, 100);
  };
  // 修改br等一些特殊字符的错误节点
  var replaceChars = function (html) {
    var ele = html;
    ele = ele.replace(/<</g, '<');
    ele = ele.replace(/>br/g, '><br');
    return ele;
  };
  function renderDiff(curItem, maxVersionItem) {
    var htmlToPlaintext = function (text) {
      return text;
    };
    var createDom = function (diff, nodeId) {
      var fragment = document.createDocumentFragment();
      for (var i = 0; i < diff.length; i++) {
        if (diff[i].added && diff[i + 1] && diff[i + 1].removed) {
          var swap = diff[i];
          diff[i] = diff[i + 1];
          diff[i + 1] = swap;
        };
        var node;
        if (diff[i].removed) {
          node = document.createElement('del');
          node.appendChild(document.createTextNode(diff[i].value));
        } else if (diff[i].added) {
          node = document.createElement('ins');
          node.appendChild(document.createTextNode(diff[i].value));
        } else {
          node = document.createTextNode(diff[i].value);
        }
        fragment.appendChild(node);
      };
      var fragmentNode = $(nodeId).html(fragment);
      var html = fragmentNode[0].innerHTML;
      html = replaceChars(he.decode(html));
      $(nodeId).html(html);
    };
    // diffChars，diffWords，diffLines
    var showStyle = "diffWords";
    createDom(JsDiff[showStyle](htmlToPlaintext(curItem.CheckItemContent), htmlToPlaintext(maxVersionItem.CheckItemContent)), '#afterContent');
    createDom(JsDiff[showStyle](htmlToPlaintext(curItem.Penalty), htmlToPlaintext(maxVersionItem.Penalty)), '#afterPenalty');
    createDom(JsDiff[showStyle](htmlToPlaintext(curItem.ReasonCodes), htmlToPlaintext(maxVersionItem.ReasonCodes)), '#afterReasonCodes');
    createDom(JsDiff[showStyle]((curItem.Importance).toString(), (maxVersionItem.Importance).toString()), '#afterImportance');
    $scope.diffCheckItem.diffElement.Version = maxVersionItem.Version;
    $scope.diffCheckItem.curElement = items.curItem;
    restSize();
    $scope.$apply();
  };
  // 老版，不支持换行高亮，暂不删除
  var renderDiff222 = function (curItem, maxVersionItem) {
    // 暂时什么都不处理
    function htmlToPlaintext(text) {
      // return he.encode(text);
      return text;
    };
    // 启用对比插件
    var dmp = new diff_match_patch();
    dmp.Diff_Timeout = 1;
    dmp.Diff_EditCost = 4;
    var diffData = {};
    diffData.CheckItemContent = dmp.diff_main(htmlToPlaintext(curItem.CheckItemContent), htmlToPlaintext(maxVersionItem.CheckItemContent));
    diffData.Penalty = dmp.diff_main(htmlToPlaintext(curItem.Penalty), htmlToPlaintext(maxVersionItem.Penalty));
    diffData.ReasonCodes = dmp.diff_main(htmlToPlaintext(curItem.ReasonCodes), htmlToPlaintext(maxVersionItem.ReasonCodes));
    diffData.Importance = dmp.diff_main((curItem.Importance).toString(), (maxVersionItem.Importance).toString());
    var doSetTimeout = function (obj) {
      setTimeout(function () {
        dmp.diff_cleanupSemantic(diffData[obj]);
        var ele = dmp.diff_prettyHtml(diffData[obj]);
        // 去除乱七八糟的符号
        ele = ele.replace(/&para;/g, '');
        ele = ele.replace(/<br>/g, '');
        // 给字符码转义成正常的html标签
        $scope.diffCheckItem.diffElement[obj] = he.decode(ele);
        // 数据改变后必须手动刷新界面
        $scope.$apply();
      }, 20);
    };
    for (obj in diffData) {
      doSetTimeout(obj);
    };
    $scope.diffCheckItem.diffElement.Version = maxVersionItem.Version;
    $scope.diffCheckItem.curElement = items.curItem;
    restSize();
    $scope.$apply();
  };
  $scope.cancel = function () {
    $modalInstance.dismiss("cancel");
  };
  var init = function () {
    $scope.diffCheckItem = {
      diffElement: {},
      curElement: {}
    };
    setTimeout(function () {
      $(".modal").css({
        background: "#f0f0f0",
        width: "1000px",
        left: "34%"
      });
      $(".modal-body").css({
        padding: "0px 15px 15px 15px"
      });
      renderDiff(items.curItem, items.maxVersionItem);
    }, 60);
  };
  init();
});