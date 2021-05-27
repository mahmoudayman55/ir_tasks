import 'screens/home.dart';
import 'package:flutter/material.dart';
void main(){
  runApp(MyApp());

}
class MyApp extends StatelessWidget{

  @override
  Widget build(BuildContext context) {
    return new MaterialApp(
      title: 'navigation',
      routes:<String,WidgetBuilder>{
        //all pages
        '/home':(BuildContext contet)=>new Home(),

      } ,
      home: new Home(),//first page
    );
  }
}