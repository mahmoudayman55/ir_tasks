import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:stemmer/stemmer.dart';

import 'dart:io';

class Home extends StatefulWidget {
  @override
  _homeState createState() {
    return new _homeState();
  }
}

class _homeState extends State {
  bool visabilty = false;
  TextEditingController _path =
      new TextEditingController(text: Directory.current.path);
  TextEditingController _query = new TextEditingController();
  Directory _directory = Directory.current;
  String numbOfFiles = "0";
  List<String> Terms = [];

  List postingList = [];
  Map<String, List> Term_posting_list = new Map<String, List>();
  List freqs = [];
  List<dynamic> IDS = [];
  List<FileSystemEntity> files;
  List<File> TextFiles = [];
  List<Text> ids = [];
  List<Text> terms = [];
  List<Text> freq = [];
  List<Text> collecfreqs = [];
  List<Text> SearchResults = [];
  List<Text> Term_posting_keys = [];
  List<Row> postinglistRow = [];
  List<Text> value = [];
  List<dynamic> Freq = [];
  int freq_counter = 0;
  int collec_freq_counter = 0;
  PorterStemmer stemmer = PorterStemmer();
  List<String> operators = ["and", "or", "not"];


  void clear() {
    setState(() {
      _query.text="";
      SearchResults = [
        Text(
          "Documents matching your search :",
          style: new TextStyle(color: Colors.blueAccent),
        )
      ];
      visabilty = false;
      TextFiles.clear();
      Freq.clear();
      freq.clear();
      postinglistRow.clear();
      Term_posting_keys.clear();
      ids.clear();
      collecfreqs.clear();
      terms.clear();
      freqs.clear();
    });
  }

  void invertedIndex() {
    setState(() {
      SearchResults = [
        Text(
          "Documents matching your search :",
          style: new TextStyle(color: Colors.blueAccent),
        )
      ];
      visabilty = true;
      _query.text="";

      freqs.clear();
      collecfreqs.clear();
      Freq.clear();
      freq.clear();
      TextFiles.clear();
      Terms.clear();
      IDS.clear();
      terms.clear();
      ids.clear();
      postinglistRow.clear();
      Term_posting_keys.clear();
      freq.add(Text("freq", style: TextStyle(color: Colors.blueAccent)));
      IDS = ["ID"];
      Terms = ["Term"];
      postinglistRow.add(Row(
        children: [
          Text("Posting list", style: TextStyle(color: Colors.blueAccent))
        ],
      ));
      _directory = Directory(_path.text);
      files = _directory.listSync(recursive: false);

      files.forEach((file) {
        if (file.path.endsWith('.txt')) {
          TextFiles.add(file);
        }
      });

      for (int i = 0; i < TextFiles.length; i++) {
        List Terms_of_file = TextFiles[i].readAsStringSync().split(' ');
        for (String term in Terms_of_file) {
          freq_counter = 0;
          for (String term2 in Terms_of_file) {
            if (term == term2) {
              freq_counter++;
            }
          }
          Freq.add(freq_counter);
          freq.add(new Text(freq_counter.toString()));
          Terms.add(term);
          IDS.add(i + 1);
        }
      }

      for (String term in Terms) {
        terms.add(Text(term.toString()));
      }
      terms[0] = new Text("Terms", style: TextStyle(color: Colors.blueAccent));
      for (var id in IDS) {
        ids.add(Text(id.toString()));
      }
      ids[0] = new Text("ID", style: TextStyle(color: Colors.blueAccent));
      List<String> uniqueTerms = Terms.toSet().toList();
      uniqueTerms.sort();
      for (String term in uniqueTerms) {
        List<String> postinglist = [];
        for (int i = 0; i < TextFiles.length; i++) {
          List Terms_of_file = TextFiles[i].readAsStringSync().split(' ');
          if (Terms_of_file.contains(term)) {
            postinglist.add((i + 1).toString());
          }
          Term_posting_list[term] = postinglist;
        }
      }

      for (String term in Term_posting_list.keys.toList()) {
        Term_posting_keys.add(new Text(term));
      }
      Term_posting_keys[0] =
          new Text("Term", style: TextStyle(color: Colors.blueAccent));

      for (List list in Term_posting_list.values.toList()) {
        List<Text> DocsIDs = [];
        for (var term in list) {
          DocsIDs.add(Text(term.toString() + "  "));
        }
        postinglistRow.add(Row(children: DocsIDs));
      }

      for (String term in uniqueTerms) {
        collec_freq_counter = 0;
        for (int i = 0; i < TextFiles.length; i++) {
          List Terms_of_file = TextFiles[i].readAsStringSync().split(' ');
          for (String term2 in Terms_of_file) {
            if (term == term2) {
              collec_freq_counter++;
            }
          }
        }
        freqs.add(collec_freq_counter);
        collecfreqs.add(Text(collec_freq_counter.toString()));
      }

      collecfreqs[0] =
          Text("freqs", style: TextStyle(color: Colors.blueAccent));
    });
  }

  _showAlertDialog(String tilte, String content) {
    showDialog(
        context: context,
        builder: (_) => new AlertDialog(
              title: new Text(tilte),
              content: new Text(content),
              actions: <Widget>[
                ElevatedButton(
                  child: Text('Close'),
                  onPressed: () {
                    Navigator.of(context).pop();
                  },
                )
              ],
            ));
  }

  void search() {
    setState(() {
      SearchResults = [
        Text(
          "Documents matching your search :",
          style: new TextStyle(color: Colors.blueAccent),
        )
      ];
      List<String> splittedQuery = _query.text.split(" ");
      print(splittedQuery[1]);
      if ((!operators.contains(stemmer.stem(splittedQuery[1])))||splittedQuery.length>3) {
        _showAlertDialog("Enter valid query!", "You can only enter a two-word query and one operator");
      } else {
        String word1 = stemmer.stem(splittedQuery[0].toString());
        String operator = stemmer.stem(splittedQuery[1].toString());
        String word2 = stemmer.stem(splittedQuery[2].toString());
        for (int i = 0; i < TextFiles.length; i++) {
          List<String> terms_of_file =
              TextFiles[i].readAsStringSync().split(" ");
          List<String> stemmedTerms = [];

          for (String term in terms_of_file) {
            stemmedTerms.add(stemmer.stem(term));
          }
          switch (operator) {
            case "and":
              if (stemmedTerms.contains(word1) &&
                  stemmedTerms.contains(word2)) {
                SearchResults.add(Text("DOC ${i + 1} , "));
              }
              break;
            case "or":
              if (stemmedTerms.contains(word1) ||
                  stemmedTerms.contains(word2)) {
                SearchResults.add(Text("DOC ${i + 1} , "));
              }
              break;
            case "not":
              if (stemmedTerms.contains(word1) &
                  !stemmedTerms.contains(word2)) {
                SearchResults.add(Text("DOC ${i + 1} , "));
              }
              break;
          }
        }
        if (SearchResults.length<=1){
          _showAlertDialog("Alert !", "No document matches your query");
        }
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: new AppBar(
        title: new Text("Inverted Index"),
      ),
      body: new Container(
        padding: new EdgeInsets.all(32.0),
        child: new Center(
          child: new Column(
            children: [
              new TextField(
                decoration: new InputDecoration(
                    labelText: "Enter the path of the folder"),
                controller: _path,
              ),
              SizedBox(
                height: 25,
              ),
              new Container(
                child: new Row(
                  children: [
                    new ElevatedButton(
                        onPressed: () => invertedIndex(), child: Text("click")),
                    SizedBox(
                      width: 20,
                    ),
                    new ElevatedButton(
                        onPressed: () => clear(), child: Text("clear")),
                  ],
                ),
              ),
              SizedBox(
                height: 25,
              ),
              Row(
                children: [
                  Container(
                    padding: EdgeInsets.all(10),
                    decoration: new BoxDecoration(
                        color: Colors.white,
                        borderRadius: BorderRadius.circular(10),
                        boxShadow: [
                          new BoxShadow(color: Colors.grey, blurRadius: 7)
                        ]),
                    child: Row(
                      children: [
                        Column(
                          children: terms,
                        ),
                        SizedBox(
                          width: 20,
                        ),
                        Column(children: ids),
                      ],
                    ),
                  ),
                  SizedBox(
                    width: 200,
                  ),
                  Container(
                    padding: EdgeInsets.all(10),
                    decoration: new BoxDecoration(
                        color: Colors.white,
                        borderRadius: BorderRadius.circular(10),
                        boxShadow: [
                          new BoxShadow(color: Colors.grey, blurRadius: 7)
                        ]),
                    child: Row(
                      children: [
                        Column(
                          children: terms,
                        ),
                        SizedBox(
                          width: 20,
                        ),
                        Column(children: ids),
                        SizedBox(
                          width: 20,
                        ),
                        Column(children: freq),
                      ],
                    ),
                  ),
                  SizedBox(
                    width: 200,
                  ),
                  Container(
                    padding: EdgeInsets.all(10),
                    decoration: new BoxDecoration(
                        color: Colors.white,
                        borderRadius: BorderRadius.circular(10),
                        boxShadow: [
                          new BoxShadow(color: Colors.grey, blurRadius: 7)
                        ]),
                    child: Row(
                      children: [
                        Column(
                          children: Term_posting_keys,
                        ),
                        SizedBox(
                          width: 20,
                        ),
                        Column(
                          children: postinglistRow,
                        ),
                        SizedBox(
                          width: 20,
                        ),
                        Column(
                          children: collecfreqs,
                        )
                      ],
                    ),
                  ),
                ],
              ),
              SizedBox(
                height: 50,
              ),
              new TextField(
                enabled: visabilty,
                controller: _query,
                decoration: new InputDecoration(
                    labelText: "Enter your query",
                    hintText: "Example: Sky And sun"),
              ), SizedBox(height: 10,),
              ElevatedButton(onPressed: () => search(), child: Text("Search")),
              new Row(
                children: SearchResults,
              ),Text("Mahmoud Ayman Elghalban")
            ],
          ),
        ),
      ),
    );
  }
}
