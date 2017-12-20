function recordTypeController($scope, recordResource, $routeParams, $location, notificationsService, overlayHelper, formHelper, eventsService) {

    $scope.validationFields = {
    };



    $scope.SubmitData = function () {
        //alert($('#txtId').val());
        if ($('#txtModuleName_u').val() == '' || $('#txtModuleName_u').val() == null)
        {
            alert("Label is Must");
            return false;
        }
        if ($('#txtParentCode').val() == $('#txtPackageId_u').val())
        {
            alert("Can't used the same Id as your Father Class Id");
            return false;
        }
        var _recordtype = { Id: $('#txtId').val(), Code: $('#txtPackageId_u').val(), Category: $('#txtCategory_u').val(), Label: $('#txtModuleName_u').val(), LabelDes: $('#txtSummary_u').val(), ParentCode: $('#txtParentCode').val(), Status: 0, Sort: 1, Depth: $('#txtDepth').val() };


        recordResource.AddorUpdateRecordType(_recordtype).then(function (response) {
            if (response.data.data) {
                InitForm();
                closeDiv();
            }
        });
    }

    $scope.PageInit = InitForm;

    InitForm();

    function InitForm() {
        //alert($('#packages').html());
        recordResource.GetAllType().then(function (response) {
            if (response.data.data) {
                var _all = response.data.data;
                var _html = "<table><tbody>";
                
                _html += "<tr><th style='width: 100px'>ID</th><th style='width: 80px'>Category</th><th style='width: 200px'>Label</th><th>Summary</th><th style='width: 60px'>Edit</th><th style='width: 60px'>Add</th><th style='width: 60px'>Delete</th></tr>";

                var m = getSelect("0", _all);
                //alert(JSON.stringify(m));
                for (var i = 0; i < m.length; i++) {

                    _html += '<tr style="background: #d9d9d9;font-weight: bold">';
                    _html += '<td>' + m[i].Code + '</td>';
                    _html += '<td style="text-align: center">' + m[i].Category + '</td>';
                    _html += '<td>' + m[i].Label + '</td>';
                    _html += '<td>' + m[i].LabelDes + '</td>';
                    _html += '<td style="text-align: center"><a data-action="edit" href="javascript:;" data-hid="' + m[i].Id + '" data-hpcode="' + m[i].ParentCode + '" data-hidDepth="' + m[i].Depth + '"  data-category="Module" data-id="' + m[i].Code + '" data-name="' + m[i].Label + '" data-abbr="' + m[i].Label + '" data-sum="' + m[i].LabelDes + '" onclick="ShowDiv(this)">Edit</a></td>';
                    _html += '<td style="text-align: center"><a data-action="add" href="javascript:;" data-id="' + m[i].Code + '" data-category="Topic" data-depth="2" onclick="ShowDivAdd(this)">Append</a></td>';
                    _html += '<td style="text-align: center"><a data-action="delete" href="javascript:;" data-id="' + m[i].Code + '" data-hid="' + m[i].Id + '" onclick="DelData(' + m[i].Id + ')" >Delete</a></td></tr>';

                    //取二级分类
                    var t = getSelect(m[i].Code, _all);
                    for (var j = 0; j < t.length; j++)
                    {
                        _html += '<tr style="background: #f2f2f2;"> ';
                        _html += '<td>' + t[j].Code + '</td> <td style="text-align: center">' + t[j].Category + '</td>';
                        _html += '<td>' + t[j].Label + '</td> <td>' + t[j].LabelDes + '</td> ';
                        _html += '<td style="text-align: center"><a data-action="edit" href="javascript:;" data-hid="' + t[j].Id + '" data-hpcode="' + t[j].ParentCode + '" data-hidDepth="' + t[j].Depth + '"  data-category="' + t[j].Category + '" data-id="' + t[j].Code + '" data-name="' + t[j].Label + '" data-abbr="' + t[j].Label + '" data-sum="' + t[j].LabelDes + '" onclick="ShowDiv(this)">Edit</a></td> ';
                        _html += '<td style="text-align: center"><a data-action="add" href="javascript:;" data-id="' + t[j].Code + '" data-category="Sub topic"  data-depth="3" onclick="ShowDivAdd(this)">Append</a></td> ';
                        _html += '<td style="text-align: center"><a data-action="delete" href="javascript:;" data-id="' + t[j].Code + '" data-hid="' + t[j].Id + '" onclick="DelData(' + t[j].Id + ')">Delete</a></td></tr>';

                        //取三级分类
                        var s = getSelect(t[j].Code, _all);
                        for (var k = 0; k < s.length; k++)
                        {
                            _html += '<tr><td>' + s[k].Code + '</td>';
                            _html += '<td style="text-align: center">' + s[k].Category + '</td> ';
                            _html += '<td>' + s[k].Label + '</td> <td>' + s[k].LabelDes + '</td>';
                            _html += '<td style="text-align: center">';
                            _html += '<a data-action="edit" href="javascript:;" data-hid="' + s[k].Id + '"  data-hpcode="' + s[k].ParentCode + '" data-hidDepth="' + s[k].Depth + '" data-category="' + s[k].Category + '" data-id="' + s[k].Code + '" data-name="' + s[k].Label + '" data-abbr="' + s[k].Label + '" data-sum="' + s[k].LabelDes + '" onclick="ShowDiv(this)">Edit</a></td> ';
                            _html += '<td style="text-align: center"></td>';
                            _html += '<td style="text-align: center"><a data-action="delete" href="javascript:;" data-id="' + s[k].Code + '" data-hid="' + s[k].Id + '" onclick="DelData(' + s[k].Id + ')">Delete</a></td></tr>';
                        }
                    }
                }

                _html += "</tbody></table>";
                $('#packages').html(_html);

            }
        });
    }

    function getSelect(_ParentCode, arr) {
        arr = arr.filter(function (inner, j) {
            return _ParentCode == inner.ParentCode
        })
        return arr
    }




}

angular.module("umbraco").controller('recordTypeController', recordTypeController);
