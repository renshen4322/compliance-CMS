angular.module("umbraco.directives")
    .directive('umbMediaPicker', function (dialogService, entityResource, overlayHelper) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: '/App_Plugins/SEACompliance/js/umbMediaPicker/umbmediapicker.html',
            require: "ngModel",
            link: function (scope, element, attr, ctrl) {
                ctrl.$render = function () {
                    if (ctrl.$viewValue != null && ctrl.$viewValue.length > 0) {
                        var array = ctrl.$viewValue.split("/");
                        var filename = "";
                        var src = ctrl.$viewValue; //media/1055/111.png
                        if (array.length > 0) {
                            filename = array.slice(array.length - 1)[0];
                        }
                        scope.node = { properties: [{ value: { src: src } }], name: filename }
                    }
                    /*
                    var val = parseInt(ctrl.$viewValue);
                    if (val && angular.isNumber(val) && val > 0) {
                        entityResource.getById(val, "Media").then(function (item) {
                            scope.node = item;
                        });
                    }*/
                };

                scope.openMediaPicker = function () {
                    overlayHelper.registerOverlay();
                    dialogService.mediaPicker({ callback: populatePicture, closeCallback: unregisterOverlay });
                }

                scope.removePicture = function () {
                    scope.node = undefined;
                    updateModel();
                }

                function populatePicture(item) {
                    scope.node = item;
                    unregisterOverlay();
                    updateModel(scope.node.properties[0].value.src);
                }

                function updateModel(src) {
                    ctrl.$setViewValue(src);
                }

                function unregisterOverlay(){
                    overlayHelper.unregisterOverlay();
                }
            }
        };
    });