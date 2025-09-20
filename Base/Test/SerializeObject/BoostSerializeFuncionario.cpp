#include "BoostSerializeFuncionario.h"
template<class Archive>

void Funcionario::serialize(Archive& archive, const unsigned int version) {
              archive & name;
              archive & age;
	      //cout << "version: " << version << endl;
              if(version>0) {
		      archive & company;
		      archive & section;
	      }
}
void Funcionario::toStr() {
              cout << name << "," << age << "," << company << "," << section << endl;
}

void Funcionario::save(ostream &OStream){
              boost::archive::binary_oarchive oa(OStream);
              oa & *(this);
}

void Funcionario::load(string strData){
              std::istringstream iss(strData);
              boost::archive::binary_iarchive ia(iss);
              ia & *(this);
}

BOOST_CLASS_VERSION(Funcionario, 1)

