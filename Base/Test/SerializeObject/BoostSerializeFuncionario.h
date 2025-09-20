#ifndef __BOOST_SERIALIZE_H
#define __BOOST_SERIALIZE_H
#include<iostream>
#include<sstream>
#include<string>
#include<boost/archive/binary_iarchive.hpp>
#include <boost/archive/binary_oarchive.hpp>

using namespace std;

class Funcionario {
private:
       friend class boost::serialization::access;
       string name;
       int age;
       string company;
       string section;
       template<class Archive>
       void serialize(Archive& , const unsigned int);

public:

       Funcionario(){ }
       Funcionario(string n, int a, string c, string s) :name(n), age(a), company(c), section(s) { }
       ~Funcionario() { }
       
       void toStr();
       void save(ostream &);
       void load(string);
};

#endif

