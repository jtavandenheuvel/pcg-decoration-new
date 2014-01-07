#include "cpp_file.h"  

#include <iostream>

#include<boost\chrono.hpp>
#include<boost/shared_ptr.hpp>
#include<CGAL/Exact_predicates_inexact_constructions_kernel.h>
#include<CGAL/Polygon_2.h>
#include<CGAL/Polygon_with_holes_2.h>
#include<CGAL/create_straight_skeleton_2.h>

#include "print.h"


typedef CGAL::Exact_predicates_inexact_constructions_kernel K ;
typedef K::Point_2                   Point ;
typedef CGAL::Polygon_2<K>           Polygon_2 ;
typedef CGAL::Straight_skeleton_2<K> Ss ;
typedef boost::shared_ptr<Ss> SsPtr ;

cpp_file::cpp_file(void)  
{  
}  
  
cpp_file::~cpp_file(void)  
{  
}  
  
void cpp_file::times2(float* input, float* output, int lenght)  
{ 
  //auto start = boost::chrono::high_resolution_clock::now();

  Polygon_2 poly ;
  for (int n=0; n < lenght; n=n+2) {
	  poly.push_back( Point(input[n],input[n+1]) ) ;
  }

  // Calculate straight skeleton
  SsPtr iss = CGAL::create_interior_straight_skeleton_2(poly.vertices_begin(), poly.vertices_end());

  // Set data in output array
  typedef CGAL::Straight_skeleton_2<K> Ss ;
  typedef Ss::Halfedge_const_iterator Halfedge_const_iterator ;

  // Count size for output array
  int count = 0;
  for ( Halfedge_const_iterator i = iss->halfedges_begin(); i != iss->halfedges_end(); ++i )
  {
	  if(i->is_bisector())
	  {
		float x1 = i->opposite()->vertex()->point().x();
		float y1 = i->opposite()->vertex()->point().y();
		float x2 = i->vertex()->point().x();
		float y2 = i->vertex()->point().y();
	    output[count++] = x1;
		output[count++] = y1;
		output[count++] = x2;
		output[count++] = y2;
	  }
  }
  //auto end = boost::chrono::high_resolution_clock::now();
  //auto elapsed = boost::chrono::duration_cast<boost::chrono::milliseconds>(end - start);

  //std::cout << "Elapsed Time: " << elapsed << "\n"; // prints Output sentence on screen
}  
